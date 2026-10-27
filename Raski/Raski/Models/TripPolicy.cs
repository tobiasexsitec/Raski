namespace Raski.Models;

/// <summary>A rule for the trip, e.g. what applies on cancellation. Managed by the trip owner/admins.</summary>
public sealed class TripPolicy
{
    public const int MaxTitleLength = 100;
    public const int MaxTextLength = 5000;

    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
    public string CreatedAt { get; set; } = "";
    public string UpdatedAt { get; set; } = "";

    /// <summary>Uid → ISO timestamp for participants who have accepted the current version.</summary>
    public Dictionary<string, string> AcceptedBy { get; set; } = [];

    public bool IsAcceptedBy(string? uid) =>
        !string.IsNullOrEmpty(uid) && AcceptedBy.ContainsKey(uid);
}
