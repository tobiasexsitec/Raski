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
    /// Adds a member by email. Returns true when an existing user was added,
    /// false when a pending participant without an account was added. The pending
    /// participant is linked automatically when they sign in with the same email.
    /// </summary>
    Task<bool> AddMemberAsync(string tripId, string email, string? displayName = null, CancellationToken ct = default);

    /// <summary>Overrides the signed-in user's alcohol preference for one trip only.</summary>
    Task SetMyAlcoholAsync(string tripId, AlcoholPreference preference, CancellationToken ct = default);

    Task RemoveMemberAsync(string tripId, string uid, CancellationToken ct = default);
}
