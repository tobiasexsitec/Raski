namespace Raski.Models;

public sealed record AmountPerUnit(string Unit, decimal Amount);

public static class ExtraCategories
{
    public const string Snacks = "fika";
    public const string Other = "ovrigt";

    public static string Display(string category) =>
        category == Snacks ? "Fika/Snacks" : "Övrigt";

    public static string Normalize(string? category) =>
        category == Snacks ? Snacks : Other;
}

public sealed class ShoppingListItem
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public List<string> Tags { get; set; } = [];
    public List<AmountPerUnit> Amounts { get; set; } = [];
    public List<string> UsedInMeals { get; set; } = [];
    public bool Checked { get; set; }

    /// <summary>
    /// The "övrigt" entries behind this row. Only set when the row comes solely from
    /// övrigt; rows shared with meals are listed with the meals instead.
    /// </summary>
    public List<string> ExtraIds { get; set; } = [];

    public bool IsExtra => ExtraIds.Count > 0;

    /// <summary>Which extras section the row belongs to; see <see cref="ExtraCategories"/>.</summary>
    public string ExtraCategory { get; set; } = ExtraCategories.Other;

    /// <summary>The tag used when grouping the shopping list.</summary>
    public string PrimaryTag =>
        Tags.Count > 0 ? Tags[0] : IngredientTags.Uncategorized;
}
