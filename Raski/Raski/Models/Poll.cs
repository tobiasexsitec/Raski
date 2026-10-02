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
    public List<string> OptionIds { get; set; } = [];
}
