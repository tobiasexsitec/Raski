using System.Text.Json.Serialization;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class HouseholdService(FirebaseInterop interop, IAuthService authService) : IHouseholdService
{
    // Firestore limits array-contains-any to 30 values.
    private const int MaxArrayContainsAny = 30;

    private string CurrentUid =>
        authService.Current?.Uid ?? throw new InvalidOperationException("Ingen inloggad användare.");

    public async Task<Household?> GetMineAsync(CancellationToken ct = default)
    {
        var uid = CurrentUid;
        var households = await QueryForMemberAsync(uid, ct);

        if (households.Count <= 1)
        {
            return households.FirstOrDefault();
        }

        // A partner who linked us has already merged our children into the shared
        // household, so the smaller ones left behind are cleaned up here.
        var keep = PickPrimary(households);

        foreach (var stale in households.Where(h => h.Id != keep.Id))
        {
            await LeaveAsync(stale, uid, ct);
        }

        return keep;
    }

    public async Task<IReadOnlyList<Household>> GetForMembersAsync(IEnumerable<string> uids, CancellationToken ct = default)
    {
        var all = new Dictionary<string, Household>();

        foreach (var chunk in uids.Distinct().Chunk(MaxArrayContainsAny))
        {
            var documents = await interop.QueryAsync<HouseholdDocument>("households", new FirestoreQuery
            {
                Where = [FirestoreFilter.ArrayContainsAny("memberUids", chunk)]
            }, ct);

            foreach (var document in documents)
            {
                all[document.Id] = document.ToModel();
            }
        }

        return [.. all.Values];
    }

    public async Task<Household> SaveChildAsync(Child child, CancellationToken ct = default)
    {
        var household = await GetMineAsync(ct) ?? await CreateAsync([CurrentUid], [], ct);

        if (string.IsNullOrEmpty(child.Id))
        {
            child.Id = Guid.NewGuid().ToString("N");
        }

        child.Name = child.Name.Trim();
        child.Allergies = AllergyNames.Distinct(child.Allergies);

        var index = household.Children.FindIndex(c => c.Id == child.Id);

        if (index >= 0)
        {
            household.Children[index] = child;
        }
        else
        {
            household.Children.Add(child);
        }

        await SaveChildrenAsync(household, ct);
        return household;
    }

    public async Task<Household?> RemoveChildAsync(string childId, CancellationToken ct = default)
    {
        var household = await GetMineAsync(ct);

        if (household is null || household.Children.RemoveAll(c => c.Id == childId) == 0)
        {
            return household;
        }

        await SaveChildrenAsync(household, ct);
        return household;
    }

    public async Task<Household> LinkPartnerAsync(string email, CancellationToken ct = default)
    {
        var uid = CurrentUid;
        var emailKey = email.Trim().ToLowerInvariant();

        var matches = await interop.QueryAsync<UserDocument>("users", new FirestoreQuery
        {
            Where = [FirestoreFilter.Equal("email", emailKey)],
            Limit = 1
        }, ct);

        var partner = matches.FirstOrDefault()
            ?? throw new InvalidOperationException($"Hittade ingen användare med e-postadressen {emailKey}.");

        if (partner.Id == uid)
        {
            throw new InvalidOperationException("Du kan inte koppla ihop dig med dig själv.");
        }

        var mine = await GetMineAsync(ct);

        if (mine?.MemberUids.Contains(partner.Id) == true)
        {
            return mine;
        }

        var partnerHouseholds = await QueryForMemberAsync(partner.Id, ct);

        if (partnerHouseholds.Any(h => h.MemberUids.Any(m => m != partner.Id)))
        {
            throw new InvalidOperationException(
                $"{partner.DisplayName ?? emailKey} är redan ihopkopplad med någon annan.");
        }

        mine ??= await CreateAsync([uid], [], ct);

        var childIds = mine.Children.Select(c => c.Id).ToHashSet();
        mine.Children.AddRange(partnerHouseholds
            .SelectMany(h => h.Children)
            .Where(c => childIds.Add(c.Id)));
        mine.MemberUids.Add(partner.Id);

        await interop.UpdateDocumentAsync($"households/{mine.Id}", new
        {
            memberUids = mine.MemberUids,
            children = mine.Children.Select(ChildDocument.From).ToArray(),
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);

        return mine;
    }

    public async Task<Household?> UnlinkAsync(CancellationToken ct = default)
    {
        var uid = CurrentUid;
        var shared = await GetMineAsync(ct);

        if (shared is null || shared.MemberUids.Count <= 1)
        {
            return shared;
        }

        var own = await CreateAsync([uid], shared.Children, ct);
        await interop.RemoveFromArrayAsync($"households/{shared.Id}", "memberUids", uid, ct);

        return own;
    }

    public async Task<IReadOnlySet<string>> GetExcludedChildIdsAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<IdDocument>($"trips/{tripId}/excludedChildren", ct: ct);
        return documents.Select(d => d.Id).ToHashSet();
    }

    public Task SetChildAttendingAsync(string tripId, string childId, bool attending, CancellationToken ct = default) =>
        attending
            ? interop.DeleteDocumentAsync($"trips/{tripId}/excludedChildren/{childId}", ct)
            : interop.SetDocumentAsync($"trips/{tripId}/excludedChildren/{childId}", new
            {
                excludedByUid = CurrentUid,
                updatedAt = FirestoreFormat.UtcNow()
            }, ct: ct);

    /// <summary>When a user is briefly in several households, the one shared with most members wins.</summary>
    public static Household PickPrimary(IEnumerable<Household> households) =>
        households
            .OrderByDescending(h => h.MemberUids.Count)
            .ThenBy(h => h.Id, StringComparer.Ordinal)
            .First();

    private async Task<List<Household>> QueryForMemberAsync(string uid, CancellationToken ct)
    {
        var documents = await interop.QueryAsync<HouseholdDocument>("households", new FirestoreQuery
        {
            Where = [FirestoreFilter.ArrayContains("memberUids", uid)]
        }, ct);

        return [.. documents.Select(d => d.ToModel())];
    }

    private async Task<Household> CreateAsync(List<string> memberUids, List<Child> children, CancellationToken ct)
    {
        var now = FirestoreFormat.UtcNow();

        var id = await interop.AddDocumentAsync("households", new
        {
            memberUids,
            children = children.Select(ChildDocument.From).ToArray(),
            createdAt = now,
            updatedAt = now
        }, ct);

        return new Household
        {
            Id = id,
            MemberUids = [.. memberUids],
            Children = [.. children]
        };
    }

    private Task SaveChildrenAsync(Household household, CancellationToken ct) =>
        interop.UpdateDocumentAsync($"households/{household.Id}", new
        {
            children = household.Children.Select(ChildDocument.From).ToArray(),
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);

    private Task LeaveAsync(Household household, string uid, CancellationToken ct) =>
        household.MemberUids.Count <= 1
            ? interop.DeleteDocumentAsync($"households/{household.Id}", ct)
            : interop.RemoveFromArrayAsync($"households/{household.Id}", "memberUids", uid, ct);

    private sealed class IdDocument
    {
        [JsonPropertyName("id")] public string Id { get; set; } = "";
    }
}
