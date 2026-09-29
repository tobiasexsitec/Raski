namespace Raski.Models;

/// <summary>Allergies and dietary preferences share one normalized list of names.</summary>
public static class AllergyNames
{
    public static readonly IReadOnlyList<string> Standard = ["gluten", "laktos", "vegetariskt"];

    public static string Normalize(string? value) =>
        string.Join(' ', (value ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToLowerInvariant();

    public static List<string> Distinct(IEnumerable<string?>? values) =>
        [.. (values ?? [])
            .Select(Normalize)
            .Where(v => v.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)];
}
