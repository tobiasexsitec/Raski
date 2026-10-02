using Raski.Models;

namespace Raski.Services;

public interface ITripService
{
    Task<IReadOnlyList<Trip>> GetMyTripsAsync(CancellationToken ct = default);
    Task<Trip?> GetAsync(string tripId, CancellationToken ct = default);
    Task<string> CreateAsync(Trip trip, CancellationToken ct = default);
    Task UpdateAsync(Trip trip, CancellationToken ct = default);

    Task<IReadOnlyList<TripMember>> GetMembersAsync(string tripId, CancellationToken ct = default);

    /// <summary>
    /// Adds a member by email. Returns true when the user was added directly,
    /// false when an invitation was stored for a not yet registered user.
    /// </summary>
    Task<bool> AddMemberAsync(string tripId, string email, CancellationToken ct = default);

    /// <summary>Overrides the signed-in user's alcohol preference for one trip only.</summary>
    Task SetMyAlcoholAsync(string tripId, AlcoholPreference preference, CancellationToken ct = default);

    Task RemoveMemberAsync(string tripId, string uid, CancellationToken ct = default);
}
