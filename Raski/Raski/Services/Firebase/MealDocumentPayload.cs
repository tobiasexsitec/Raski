using System.Text.Json.Serialization;
using Raski.Models;

namespace Raski.Services.Firebase;

/// <summary>
/// Public snapshot payload for realtime listeners. The internal document types
/// cannot be used here because [JSInvokable] methods must have public signatures.
/// </summary>
public sealed class MealDocumentPayload
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("date")] public string? Date { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("responsibleUid")] public string? ResponsibleUid { get; set; }
    [JsonPropertyName("responsibleName")] public string? ResponsibleName { get; set; }
    [JsonPropertyName("recipeUrl")] public string? RecipeUrl { get; set; }
    [JsonPropertyName("comment")] public string? Comment { get; set; }
    [JsonPropertyName("ingredients")] public List<MealIngredientPayload>? Ingredients { get; set; }
    [JsonPropertyName("drinks")] public List<MealDrinkPayload>? Drinks { get; set; }

    public Meal ToMeal(string tripId) => new()
    {
        Id = Id,
        TripId = tripId,
        Date = FirestoreFormat.ParseDate(Date),
        Type = MealTypeExtensions.ParseMealType(Type),
        Title = Title ?? "",
        ResponsibleUid = ResponsibleUid ?? "",
        ResponsibleName = ResponsibleName ?? "",
        RecipeUrl = RecipeUrl ?? "",
        Comment = Comment ?? "",
        Ingredients = Ingredients?.Select(i => i.ToModel()).ToList() ?? [],
        Drinks = Drinks?.Select(d => d.ToModel()).ToList() ?? []
    };
}

public sealed class MealDrinkPayload
{
    [JsonPropertyName("ingredientId")] public string? IngredientId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("quantity")] public string? Quantity { get; set; }
    [JsonPropertyName("unit")] public string? Unit { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }

    public MealDrink ToModel() => new()
    {
        IngredientId = IngredientId ?? "",
        Name = Name ?? "",
        Quantity = Quantity ?? "",
        Unit = Unit ?? "",
        Url = Url ?? ""
    };
}

public sealed class MealIngredientPayload
{
    [JsonPropertyName("ingredientId")] public string? IngredientId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("amount")] public double Amount { get; set; }
    [JsonPropertyName("unit")] public string? Unit { get; set; }

    public MealIngredient ToModel() => new()
    {
        IngredientId = IngredientId ?? "",
        Name = Name ?? "",
        Amount = (decimal)Amount,
        Unit = Unit ?? ""
    };
}
