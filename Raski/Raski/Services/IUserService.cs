using Raski.Models;

namespace Raski.Services;

public interface IUserService
{
    /// <summary>All registered users, used by the global admin management page.</summary>
    Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken ct = default);

    Task<UserProfile?> GetAsync(string uid, CancellationToken ct = default);

    /// <summary>Updates the signed-in user's phone number.</summary>
    Task SaveMyPhoneAsync(string phone, CancellationToken ct = default);

    Task SetGlobalAdminAsync(string uid, bool isGlobalAdmin, CancellationToken ct = default);
}
