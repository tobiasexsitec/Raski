namespace Raski.Models;

public enum MealType
{
    Lunch,
    Dinner
}

public static class MealTypeExtensions
{
    public static string ToFirestore(this MealType type) =>
        type == MealType.Lunch ? "lunch" : "dinner";

    public static MealType ParseMealType(string? value) =>
        string.Equals(value, "lunch", StringComparison.OrdinalIgnoreCase) ? MealType.Lunch : MealType.Dinner;

    public static string ToSwedish(this MealType type) =>
        type == MealType.Lunch ? "Lunch" : "Middag";
}

public sealed class Meal
{
    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public DateOnly Date { get; set; }
    public MealType Type { get; set; }
    public string Title { get; set; } = "";
    public string ResponsibleUid { get; set; } = "";
    public string ResponsibleName { get; set; } = "";
    public string RecipeUrl { get; set; } = "";
    public List<MealIngredient> Ingredients { get; set; } = [];
}

public sealed class MealIngredient
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public string Unit { get; set; } = "";
}
