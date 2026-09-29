namespace Raski.Models;

public sealed class Ingredient
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string NameLower { get; set; } = "";
    public List<string> Tags { get; set; } = [];
    public string? DefaultUnit { get; set; }
    public string? MergedIntoId { get; set; }

    public bool IsMerged => !string.IsNullOrEmpty(MergedIntoId);
}

/// <summary>The known ingredient properties used for shopping list grouping.</summary>
public static class IngredientTags
{
    public const string Uncategorized = "övrigt";

    public static readonly IReadOnlyList<string> All =
    [
        "kylvara",
        "grönsak",
        "torrvara",
        "frys",
        "kryddor",
        "drycker",
        "bröd",
        Uncategorized
    ];

    public static string Display(string tag) =>
        string.IsNullOrWhiteSpace(tag) ? Uncategorized : char.ToUpperInvariant(tag[0]) + tag[1..];
}
