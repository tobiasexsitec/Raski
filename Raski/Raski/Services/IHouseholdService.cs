using Raski.Models;

namespace Raski.Services;

public interface IHouseholdService
{
    /// <summary>The signed-in user's household, or null if they have none yet.</summary>
    Task<Household?> GetMineAsync(CancellationToken ct = default);

    /// <summary>Households that any of the given users belong to.</summary>
    Task<IReadOnlyList<Household>> GetForMembersAsync(IEnumerable<string> uids, CancellationToken ct = default);

    /// <summary>Adds or updates a child in the signed-in user's household, creating the household if needed.</summary>
    Task<Household> SaveChildAsync(Child child, CancellationToken ct = default);

    Task<Household?> RemoveChildAsync(string childId, CancellationToken ct = default);

    /// <summary>
    /// Links the signed-in user with the user who has the given email. The link
    /// applies immediately; the partner's children are merged into the shared household.
    /// </summary>
    Task<Household> LinkPartnerAsync(string email, CancellationToken ct = default);

    /// <summary>Leaves the shared household. The signed-in user keeps a copy of the children.</summary>
    Task<Household?> UnlinkAsync(CancellationToken ct = default);

    Task<IReadOnlySet<string>> GetExcludedChildIdsAsync(string tripId, CancellationToken ct = default);

    Task SetChildAttendingAsync(string tripId, string childId, bool attending, CancellationToken ct = default);
}
