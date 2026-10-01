namespace Raski.Models;

public enum MealType
{
    Lunch,
    Starter,
    Dinner,
    Dessert
}

public static class MealTypeExtensions
{
    public static string ToFirestore(this MealType type) => type switch
    {
        MealType.Lunch => "lunch",
        MealType.Starter => "starter",
        MealType.Dessert => "dessert",
        _ => "dinner"
    };

    public static MealType ParseMealType(string? value) => value?.ToLowerInvariant() switch
    {
        "lunch" => MealType.Lunch,
        "starter" => MealType.Starter,
        "dessert" => MealType.Dessert,
        _ => MealType.Dinner
    };

    public static string ToSwedish(this MealType type) => type switch
    {
        MealType.Lunch => "Lunch",
        MealType.Starter => "Förrätt",
        MealType.Dessert => "Efterrätt",
        _ => "Middag"
    };
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
    public string Comment { get; set; } = "";
    public List<MealIngredient> Ingredients { get; set; } = [];
}

public sealed class MealIngredient
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public string Unit { get; set; } = "";
}
