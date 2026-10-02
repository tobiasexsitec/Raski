using System.Globalization;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

/// <summary>
/// Polls live in trips/{tripId}/polls. Options and votes are separate
/// subcollections (one document each) so concurrent additions and votes
/// never overwrite each other.
/// </summary>
public sealed class PollService(FirebaseInterop interop, IAuthService authService) : IPollService
{
    private static string CollectionPath(string tripId) => $"trips/{tripId}/polls";
    private static string PollPath(string tripId, string pollId) => $"{CollectionPath(tripId)}/{pollId}";
    private static string OptionsPath(string tripId, string pollId) => $"{PollPath(tripId, pollId)}/options";
    private static string VotesPath(string tripId, string pollId) => $"{PollPath(tripId, pollId)}/votes";

    public async Task<IReadOnlyList<Poll>> GetAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<PollDocument>(CollectionPath(tripId), ct: ct);
        var polls = documents.Select(d => d.ToModel(tripId)).ToList();

        await Task.WhenAll(polls.Select(async poll =>
        {
            var optionsTask = interop.QueryAsync<PollOptionDocument>(OptionsPath(tripId, poll.Id), ct: ct);
            var votesTask = interop.QueryAsync<PollVoteDocument>(VotesPath(tripId, poll.Id), ct: ct);

            poll.Options = [.. (await optionsTask).Select(o => o.ToModel()).OrderBy(o => o.Order)];
            poll.Votes = [.. (await votesTask).Select(v => v.ToModel())];
        }));

        return polls
            .OrderByDescending(p => p.CreatedAt, StringComparer.Ordinal)
            .ToList();
    }

    public async Task<string> CreateAsync(Poll poll, IEnumerable<string> optionTexts, CancellationToken ct = default)
    {
        var data = PollData(poll);
        data["createdAt"] = FirestoreFormat.UtcNow();
        data["createdByUid"] = authService.Current?.Uid ?? "";
        data["createdByName"] = authService.Current?.DisplayName ?? "";

        var pollId = await interop.AddDocumentAsync(CollectionPath(poll.TripId), data, ct);

        var order = 0;
        foreach (var text in optionTexts)
        {
            await AddOptionAsync(poll.TripId, pollId, text, order++, ct);
        }

        return pollId;
    }

    public Task UpdateAsync(Poll poll, CancellationToken ct = default) =>
        interop.SetDocumentAsync(PollPath(poll.TripId, poll.Id), PollData(poll), ct: ct);

    public async Task<PollOption> AddOptionAsync(string tripId, string pollId, string text, int order, CancellationToken ct = default)
    {
        var option = new PollOption
        {
            Text = text.Trim(),
            Order = order,
            AddedByUid = authService.Current?.Uid ?? "",
            AddedByName = authService.Current?.DisplayName ?? ""
        };

        var data = new Dictionary<string, object?>
        {
            ["text"] = option.Text,
            ["order"] = option.Order,
            ["addedByUid"] = option.AddedByUid,
            ["addedByName"] = option.AddedByName,
            ["createdAt"] = FirestoreFormat.UtcNow()
        };

        option.Id = await interop.AddDocumentAsync(OptionsPath(tripId, pollId), data, ct);
        return option;
    }

    public Task UpdateOptionAsync(string tripId, string pollId, PollOption option, CancellationToken ct = default) =>
        interop.SetDocumentAsync($"{OptionsPath(tripId, pollId)}/{option.Id}", new Dictionary<string, object?>
        {
            ["text"] = option.Text.Trim(),
            ["order"] = option.Order,
            ["updatedAt"] = FirestoreFormat.UtcNow()
        }, ct: ct);

    public async Task<PollVote?> VoteAsync(string tripId, string pollId, IReadOnlyCollection<string> optionIds, CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Not signed in.");
        var path = $"{VotesPath(tripId, pollId)}/{uid}";

        if (optionIds.Count == 0)
        {
            await interop.DeleteDocumentAsync(path, ct);
            return null;
        }

        var vote = new PollVote
        {
            VoterUid = uid,
            VoterName = authService.Current?.DisplayName ?? "",
            OptionIds = [.. optionIds.Distinct()]
        };

        await interop.SetDocumentAsync(path, new Dictionary<string, object?>
        {
            ["voterName"] = vote.VoterName,
            ["optionIds"] = vote.OptionIds,
            ["updatedAt"] = FirestoreFormat.UtcNow()
        }, merge: false, ct: ct);

        return vote;
    }

    public async Task DeleteAsync(Poll poll, CancellationToken ct = default)
    {
        var votes = await interop.QueryAsync<PollVoteDocument>(VotesPath(poll.TripId, poll.Id), ct: ct);
        var options = await interop.QueryAsync<PollOptionDocument>(OptionsPath(poll.TripId, poll.Id), ct: ct);

        await Task.WhenAll(votes.Select(v => interop.DeleteDocumentAsync($"{VotesPath(poll.TripId, poll.Id)}/{v.Id}", ct)));
        await Task.WhenAll(options.Select(o => interop.DeleteDocumentAsync($"{OptionsPath(poll.TripId, poll.Id)}/{o.Id}", ct)));
        await interop.DeleteDocumentAsync(PollPath(poll.TripId, poll.Id), ct);
    }

    private Dictionary<string, object?> PollData(Poll poll) => new()
    {
        ["question"] = poll.Question.Trim(),
        ["allowMultiple"] = poll.AllowMultiple,
        ["maxChoices"] = poll.AllowMultiple ? Math.Max(0, poll.MaxChoices) : 1,
        ["allowCustomOptions"] = poll.AllowCustomOptions,
        ["deadline"] = poll.Deadline?.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture),
        ["closed"] = poll.IsClosed,
        ["updatedAt"] = FirestoreFormat.UtcNow(),
        ["updatedByUid"] = authService.Current?.Uid ?? ""
    };
}
