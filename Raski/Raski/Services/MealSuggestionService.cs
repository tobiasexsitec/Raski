using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class MealSuggestionService(FirebaseInterop interop, IAuthService authService) : IMealSuggestionService
{
    // Kept apart from meals so suggestions never reach the shopping list or day views.
    private static string CollectionPath(string tripId) => $"trips/{tripId}/suggestions";

    public async Task<IReadOnlyList<MealSuggestion>> GetAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<MealSuggestionDocument>(CollectionPath(tripId), ct: ct);

        return documents
            .Select(d => d.ToModel(tripId))
            .OrderBy(s => s.Title, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    public async Task<string> SaveAsync(MealSuggestion suggestion, CancellationToken ct = default)
    {
        var data = new Dictionary<string, object?>
        {
            ["title"] = suggestion.Title.Trim(),
            ["recipeUrl"] = suggestion.RecipeUrl.Trim(),
            ["comment"] = suggestion.Comment.Trim(),
            ["updatedAt"] = FirestoreFormat.UtcNow(),
            ["updatedByUid"] = authService.Current?.Uid ?? ""
        };

        if (string.IsNullOrEmpty(suggestion.Id))
        {
            data["createdAt"] = FirestoreFormat.UtcNow();
            data["createdByUid"] = authService.Current?.Uid ?? "";

            return await interop.AddDocumentAsync(CollectionPath(suggestion.TripId), data, ct);
        }

        await interop.SetDocumentAsync($"{CollectionPath(suggestion.TripId)}/{suggestion.Id}", data, ct: ct);
        return suggestion.Id;
    }

    public Task DeleteAsync(string tripId, string suggestionId, CancellationToken ct = default) =>
        interop.DeleteDocumentAsync($"{CollectionPath(tripId)}/{suggestionId}", ct);
}
