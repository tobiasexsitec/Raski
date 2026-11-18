namespace Raski.Models;

/// <summary>Users who belong together (e.g. partners) and share their children.</summary>
public sealed class Household
{
    public string Id { get; set; } = "";

    /// <summary>Chosen by the household; empty means the adults' first names are shown instead.</summary>
    public string Name { get; set; } = "";

    public List<string> MemberUids { get; set; } = [];
    public List<Child> Children { get; set; } = [];

    public string DisplayName(IEnumerable<string> adultNames) =>
        string.IsNullOrWhiteSpace(Name) ? SuggestName(adultNames) : Name.Trim();

    /// <summary>The adults' first names, e.g. "Anna och Erik".</summary>
    public static string SuggestName(IEnumerable<string> adultNames)
    {
        List<string> firstNames = [.. adultNames
            .Select(n => n.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "")
            .Where(n => n.Length > 0)
            .Distinct(StringComparer.CurrentCultureIgnoreCase)];

        return firstNames.Count <= 1
            ? firstNames.FirstOrDefault() ?? ""
            : $"{string.Join(", ", firstNames[..^1])} och {firstNames[^1]}";
    }
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
