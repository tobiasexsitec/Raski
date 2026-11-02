using Raski.Models;

namespace Raski.Services;

public interface ITripPolicyService
{
    /// <summary>All policies on the trip, oldest first.</summary>
    Task<IReadOnlyList<TripPolicy>> GetAsync(string tripId, CancellationToken ct = default);

    /// <summary>Creates the policy. Returns the new policy id.</summary>
    Task<string> CreateAsync(TripPolicy policy, CancellationToken ct = default);

    /// <summary>Saves title and text and clears all acceptances, since the content changed.</summary>
    Task UpdateAsync(TripPolicy policy, CancellationToken ct = default);

    /// <summary>Records that the current user accepts the policy.</summary>
    Task AcceptAsync(TripPolicy policy, CancellationToken ct = default);

    Task DeleteAsync(TripPolicy policy, CancellationToken ct = default);
}
