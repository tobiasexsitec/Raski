using Raski.Models;

namespace Raski.Services;

/// <summary>
/// Pure prefix filtering used by the autocomplete. Kept free of Firestore and JS
/// dependencies so the matching rules can be reasoned about and unit tested.
/// </summary>
public static class IngredientFilter
{
    /// <summary>The autocomplete only searches once the user typed this many characters.</summary>
    public const int MinimumPrefixLength = 2;

    public static string Normalize(string? value) =>
        (value ?? "").Trim().ToLowerInvariant();

    public static bool ShouldSearch(string? term) =>
        Normalize(term).Length >= MinimumPrefixLength;

    /// <summary>
    /// Matches ingredients whose name starts with the prefix, then those that
    /// contain it. Merged ingredients are never suggested so everyone converges
    /// on the surviving entry.
    /// </summary>
    public static IReadOnlyList<Ingredient> Search(
        IEnumerable<Ingredient> ingredients,
        string? prefix,
        int limit = 8)
    {
        var normalized = Normalize(prefix);

        if (normalized.Length < MinimumPrefixLength)
        {
            return [];
        }

        return ingredients
            .Where(i => !i.IsMerged)
            .Select(i => new { Ingredient = i, Rank = Rank(i.NameLower, normalized) })
            .Where(x => x.Rank >= 0)
            .OrderBy(x => x.Rank)
            .ThenBy(x => x.Ingredient.NameLower, StringComparer.Ordinal)
            .Take(limit)
            .Select(x => x.Ingredient)
            .ToList();
    }

    public static IReadOnlyList<string> SearchUnits(
        IEnumerable<string> units,
        string? prefix,
        int limit = 8)
    {
        var normalized = Normalize(prefix);

        if (normalized.Length == 0)
        {
            return units.Take(limit).ToList();
        }

        return units
            .Select(u => new { Unit = u, Rank = Rank(u.ToLowerInvariant(), normalized) })
            .Where(x => x.Rank >= 0)
            .OrderBy(x => x.Rank)
            .ThenBy(x => x.Unit, StringComparer.Ordinal)
            .Take(limit)
            .Select(x => x.Unit)
            .ToList();
    }

    /// <summary>0 for a prefix match, 1 for a substring match, -1 for no match.</summary>
    private static int Rank(string candidate, string normalizedTerm)
    {
        if (candidate.StartsWith(normalizedTerm, StringComparison.Ordinal))
        {
            return 0;
        }

        return candidate.Contains(normalizedTerm, StringComparison.Ordinal) ? 1 : -1;
    }
}
