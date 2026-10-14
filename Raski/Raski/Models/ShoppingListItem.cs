namespace Raski.Models;

public sealed record AmountPerUnit(string Unit, decimal Amount);

public sealed class ShoppingListItem
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public List<string> Tags { get; set; } = [];
    public List<AmountPerUnit> Amounts { get; set; } = [];
    public List<string> UsedInMeals { get; set; } = [];
    public bool Checked { get; set; }

    /// <summary>Set for items added directly to the shopping list rather than from a meal.</summary>
    public string? ExtraId { get; set; }

    public bool IsExtra => ExtraId is not null;

    /// <summary>The tag used when grouping the shopping list.</summary>
    public string PrimaryTag =>
        Tags.Count > 0 ? Tags[0] : IngredientTags.Uncategorized;
}
