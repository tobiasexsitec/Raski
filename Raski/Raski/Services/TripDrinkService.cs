using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class TripDrinkService(FirebaseInterop interop, IAuthService authService) : ITripDrinkService
{
    // A trip has exactly one shared drink list, so it lives in a fixed document.
    private static string DocumentPath(string tripId) => $"trips/{tripId}/drinks/default";

    public async Task<List<MealDrink>> GetAsync(string tripId, CancellationToken ct = default)
    {
        var document = await interop.GetDocumentAsync<TripDrinksDocument>(DocumentPath(tripId), ct);
        return document?.Drinks?.Select(d => d.ToModel()).ToList() ?? [];
    }

    public Task SaveAsync(string tripId, IReadOnlyList<MealDrink> drinks, CancellationToken ct = default)
    {
        var items = drinks
            .Where(d => !string.IsNullOrWhiteSpace(d.Name))
            .Select(d => new
            {
                ingredientId = d.IngredientId,
                name = d.Name.Trim(),
                quantity = d.Quantity.Trim(),
                unit = d.Unit.Trim(),
                url = d.Url.Trim()
            })
            .ToArray();

        var data = new Dictionary<string, object?>
        {
            ["drinks"] = items,
            ["updatedAt"] = FirestoreFormat.UtcNow(),
            ["updatedByUid"] = authService.Current?.Uid ?? ""
        };

        return interop.SetDocumentAsync(DocumentPath(tripId), data, merge: false, ct: ct);
    }
}
