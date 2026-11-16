using Raski.Models;

namespace Raski.Services;

public interface ICarService
{
    /// <summary>Streams the trip's cars so every participant sees changes live.</summary>
    IAsyncEnumerable<IReadOnlyList<Car>> ObserveCars(string tripId, CancellationToken ct);

    Task<string> AddAsync(string tripId, string name, IReadOnlyList<string> memberUids, CancellationToken ct = default);

    Task UpdateAsync(string tripId, Car car, CancellationToken ct = default);

    /// <summary>Removes a car. Rows assigned to it fall back to having no car.</summary>
    Task DeleteAsync(string tripId, string carId, CancellationToken ct = default);
}
