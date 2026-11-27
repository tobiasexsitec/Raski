namespace Raski.Models;

/// <summary>A poll on a trip where the travellers vote on one or more options.</summary>
public sealed class Poll
{
    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public string Question { get; set; } = "";
    public bool AllowMultiple { get; set; }
    /// <summary>Max number of choices when AllowMultiple. 0 = unlimited.</summary>
    public int MaxChoices { get; set; }
    public bool AllowCustomOptions { get; set; }
    /// <summary>Only one vote counts per household; any member may change it.</summary>
    public bool OneVotePerHousehold { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public bool IsClosed { get; set; }
    public string CreatedByUid { get; set; } = "";
    public string CreatedByName { get; set; } = "";
    public string CreatedAt { get; set; } = "";
    public List<PollOption> Options { get; set; } = [];
    public List<PollVote> Votes { get; set; } = [];

    /// <summary>Closed manually or the deadline has passed.</summary>
    public bool IsOpen(DateTimeOffset now) => !IsClosed && (Deadline is null || now < Deadline);

    public bool IsCreator(string? uid) => !string.IsNullOrEmpty(uid) && uid == CreatedByUid;

    /// <summary>The household the voter belongs to, or the voter alone when they have none.</summary>
    public static string HouseholdKey(string uid, IEnumerable<Household> households) =>
        households.FirstOrDefault(h => h.MemberUids.Contains(uid))?.Id ?? $"user:{uid}";

    /// <summary>The uids that share a vote with <paramref name="uid"/> when one vote per household applies.</summary>
    public static IReadOnlyList<string> HouseholdUids(string uid, IEnumerable<Household> households) =>
        households.FirstOrDefault(h => h.MemberUids.Contains(uid))?.MemberUids ?? [uid];

    public IEnumerable<IGrouping<string, PollVote>> VotesByHousehold(IReadOnlyCollection<Household> households) =>
        Votes.GroupBy(v => HouseholdKey(v.VoterUid, households));

    /// <summary>Households where members have voted differently, so their votes can't be merged.</summary>
    public IReadOnlyList<IGrouping<string, PollVote>> ConflictingHouseholds(IReadOnlyCollection<Household> households) =>
        [.. VotesByHousehold(households)
            .Where(g => g.Select(v => string.Join('|', v.OptionIds.Order(StringComparer.Ordinal))).Distinct().Count() > 1)];

    /// <summary>Votes that become redundant when identical votes within a household are merged.</summary>
    public IReadOnlyList<PollVote> DuplicateHouseholdVotes(IReadOnlyCollection<Household> households) =>
        [.. VotesByHousehold(households).SelectMany(g => g.Skip(1))];
}

public sealed class PollOption
{
    public string Id { get; set; } = "";
    public string Text { get; set; } = "";
    public int Order { get; set; }
    public string AddedByUid { get; set; } = "";
    public string AddedByName { get; set; } = "";
}

/// <summary>One traveller's current vote. Stored per voter so it can be changed.</summary>
public sealed class PollVote
{
    public string VoterUid { get; set; } = "";
    public string VoterName { get; set; } = "";
    public string HouseholdId { get; set; } = "";
    public List<string> OptionIds { get; set; } = [];
}
