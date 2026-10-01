using Raski.Models;

namespace Raski.Services;

public interface ITripDrinkService
{
    /// <summary>Returns the trip's shared drinks, or an empty list if none have been saved.</summary>
    Task<List<MealDrink>> GetAsync(string tripId, CancellationToken ct = default);

    Task SaveAsync(string tripId, IReadOnlyList<MealDrink> drinks, CancellationToken ct = default);
}
