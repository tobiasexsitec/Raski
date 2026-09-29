using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class BreakfastService(FirebaseInterop interop, IAuthService authService) : IBreakfastService
{
    // A trip has exactly one breakfast, so it lives in a fixed document.
    private static string DocumentPath(string tripId) => $"trips/{tripId}/breakfast/default";

    public async Task<Breakfast> GetAsync(string tripId, CancellationToken ct = default)
    {
        var document = await interop.GetDocumentAsync<BreakfastDocument>(DocumentPath(tripId), ct);
        return document?.ToModel(tripId) ?? new Breakfast { TripId = tripId };
    }

    public Task SaveAsync(Breakfast breakfast, CancellationToken ct = default)
    {
        var ingredients = breakfast.Ingredients
            .Where(i => !string.IsNullOrWhiteSpace(i.Name))
            .Select(i => new
            {
                ingredientId = i.IngredientId,
                name = i.Name.Trim(),
                amount = (double)i.Amount,
                unit = i.Unit.Trim(),
                scope = i.Scope.ToFirestore()
            })
            .ToArray();

        var data = new Dictionary<string, object?>
        {
            ["ingredients"] = ingredients,
            ["updatedAt"] = FirestoreFormat.UtcNow(),
            ["updatedByUid"] = authService.Current?.Uid ?? ""
        };

        return interop.SetDocumentAsync(DocumentPath(breakfast.TripId), data, merge: false, ct: ct);
    }
}
