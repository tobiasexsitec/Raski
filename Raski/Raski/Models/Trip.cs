namespace Raski.Models;

public sealed class Trip
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Destination { get; set; } = "";
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Notes { get; set; }
    public string OwnerUid { get; set; } = "";
    public List<string> MemberUids { get; set; } = [];

    /// <summary>Every day of the trip, inclusive. Empty when the range is invalid.</summary>
    public IEnumerable<DateOnly> Days()
    {
        for (var day = StartDate; day <= EndDate; day = day.AddDays(1))
        {
            yield return day;
        }
    }

    public bool IsOwner(string? uid) =>
        !string.IsNullOrEmpty(uid) && OwnerUid == uid;
}

public sealed class TripMember
{
    public string Uid { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhotoUrl { get; set; } = "";
    public string Role { get; set; } = MemberRoles.Member;

    public bool IsAdmin => Role == MemberRoles.Admin;
}

public static class MemberRoles
{
    public const string Admin = "admin";
    public const string Member = "member";
}
