using Raski.Models;

namespace Raski.Services;

public interface IMealService
{
    /// <summary>
    /// Streams the trip's meals, pushing a new snapshot whenever any participant
    /// changes something. The listener is torn down when enumeration stops.
    /// </summary>
    IAsyncEnumerable<IReadOnlyList<Meal>> ObserveMeals(string tripId, CancellationToken ct);

    Task<IReadOnlyList<Meal>> GetMealsAsync(string tripId, CancellationToken ct = default);
    Task<string> SaveAsync(Meal meal, CancellationToken ct = default);
    Task DeleteAsync(string tripId, string mealId, CancellationToken ct = default);
}
