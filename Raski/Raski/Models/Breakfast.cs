namespace Raski.Models;

public enum BreakfastAmountScope
{
    PerPersonAndDay,
    PerTrip
}

public static class BreakfastAmountScopeExtensions
{
    public static string ToFirestore(this BreakfastAmountScope scope) =>
        scope == BreakfastAmountScope.PerTrip ? "perTrip" : "perPersonAndDay";

    public static BreakfastAmountScope ParseBreakfastAmountScope(string? value) =>
        string.Equals(value, "perTrip", StringComparison.OrdinalIgnoreCase)
            ? BreakfastAmountScope.PerTrip
            : BreakfastAmountScope.PerPersonAndDay;

    public static string ToSwedish(this BreakfastAmountScope scope) =>
        scope == BreakfastAmountScope.PerTrip ? "för resan" : "per person och dag";
}

/// <summary>
/// The shared breakfast for a whole trip. It is eaten every day, so it is
/// defined once instead of being planned per day.
/// </summary>
public sealed class Breakfast
{
    public const string Title = "Frukost";

    public string TripId { get; set; } = "";
    public List<BreakfastIngredient> Ingredients { get; set; } = [];

    /// <summary>
    /// Expands the breakfast into a single meal with total amounts so it can be
    /// summed together with the other meals on the shopping list.
    /// </summary>
    public Meal ToMeal(int travelers, int days) => new()
    {
        TripId = TripId,
        Title = Title,
        Ingredients =
        [
            .. Ingredients.Select(i => new MealIngredient
            {
                IngredientId = i.IngredientId,
                Name = i.Name,
                Amount = i.TotalAmount(travelers, days),
                Unit = i.Unit
            })
        ]
    };
}

public sealed class BreakfastIngredient
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public string Unit { get; set; } = "";
    public BreakfastAmountScope Scope { get; set; }

    public decimal TotalAmount(int travelers, int days) =>
        Scope == BreakfastAmountScope.PerTrip
            ? Amount
            : Amount * Math.Max(travelers, 0) * Math.Max(days, 0);
}
