using Raski.Models;

namespace Raski.Services;

public interface IShoppingListService
{
    /// <summary>Builds the aggregated shopping list for a trip from its meals.</summary>
    Task<IReadOnlyList<ShoppingListItem>> BuildAsync(string tripId, CancellationToken ct = default);

    /// <summary>
    /// Streams the shared checked state so every participant sees check-offs live.
    /// Keyed by ingredient id.
    /// </summary>
    IAsyncEnumerable<IReadOnlyDictionary<string, bool>> ObserveCheckedState(string tripId, CancellationToken ct);

    /// <summary>Persists the checked state so it is shared by every participant.</summary>
    Task SetCheckedAsync(string tripId, string ingredientId, bool isChecked, CancellationToken ct = default);

    /// <summary>Gets the items that do not belong to any meal, e.g. snacks and household goods.</summary>
    Task<IReadOnlyList<ShoppingListItem>> GetExtrasAsync(string tripId, CancellationToken ct = default);

    /// <summary>Adds an item that does not belong to any meal.</summary>
    Task AddExtraAsync(string tripId, string name, decimal? amount, string unit, string category, CancellationToken ct = default);

    /// <summary>Removes an extra item together with its checked state.</summary>
    Task DeleteExtraAsync(string tripId, string extraId, CancellationToken ct = default);
}
