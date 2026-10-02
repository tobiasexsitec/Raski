using Raski.Models;

namespace Raski.Services;

public interface IPollService
{
    /// <summary>All polls on the trip, with options and votes, newest first.</summary>
    Task<IReadOnlyList<Poll>> GetAsync(string tripId, CancellationToken ct = default);

    /// <summary>Creates the poll and its initial options. Returns the new poll id.</summary>
    Task<string> CreateAsync(Poll poll, IEnumerable<string> optionTexts, CancellationToken ct = default);

    /// <summary>Saves question, settings, deadline and closed state. Votes are untouched.</summary>
    Task UpdateAsync(Poll poll, CancellationToken ct = default);

    Task<PollOption> AddOptionAsync(string tripId, string pollId, string text, int order, CancellationToken ct = default);

    Task UpdateOptionAsync(string tripId, string pollId, PollOption option, CancellationToken ct = default);

    /// <summary>Replaces the current user's vote. An empty selection removes the vote.</summary>
    Task<PollVote?> VoteAsync(string tripId, string pollId, IReadOnlyCollection<string> optionIds, CancellationToken ct = default);

    Task DeleteAsync(Poll poll, CancellationToken ct = default);
}
