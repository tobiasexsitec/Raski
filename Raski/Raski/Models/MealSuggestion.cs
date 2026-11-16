namespace Raski.Models;

/// <summary>An idea for a dish that is not yet placed on a day and has no ingredients.</summary>
public sealed class MealSuggestion
{
    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public string Title { get; set; } = "";
    public string RecipeUrl { get; set; } = "";
    public string Comment { get; set; } = "";
}
