using Raski.Models;

namespace Raski.Services;

public interface IBreakfastService
{
    /// <summary>Returns the trip's breakfast, or an empty one if none has been saved.</summary>
    Task<Breakfast> GetAsync(string tripId, CancellationToken ct = default);

    Task SaveAsync(Breakfast breakfast, CancellationToken ct = default);
}
