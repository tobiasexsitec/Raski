using Raski.Models;

namespace Raski.Services;

public interface IBringItemService
{
    Task<IReadOnlyList<BringItem>> GetAsync(string tripId, CancellationToken ct = default);

    /// <summary>Creates or updates the item and returns its id.</summary>
    Task<string> SaveAsync(BringItem item, CancellationToken ct = default);

    Task DeleteAsync(string tripId, string itemId, CancellationToken ct = default);
}
