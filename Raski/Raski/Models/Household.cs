namespace Raski.Models;

/// <summary>Users who belong together (e.g. partners) and share their children.</summary>
public sealed class Household
{
    public string Id { get; set; } = "";
    public List<string> MemberUids { get; set; } = [];
    public List<Child> Children { get; set; } = [];
}

public sealed class Child
{
    /// <summary>Kept when a household is split so a child is still counted once per trip.</summary>
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public DateOnly BirthDate { get; set; }

    /// <summary>Allergies and dietary preferences, stored normalized like on <see cref="UserProfile"/>.</summary>
    public List<string> Allergies { get; set; } = [];

    public int AgeOn(DateOnly date)
    {
        var age = date.Year - BirthDate.Year;

        if (date < BirthDate.AddYears(age))
        {
            age--;
        }

        return Math.Max(age, 0);
    }
}
