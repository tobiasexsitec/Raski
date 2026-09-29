using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class UserService(FirebaseInterop interop) : IUserService
{
    public async Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<UserDocument>("users", null, ct);

        return [.. documents
            .Select(d => d.ToModel())
            .OrderBy(u => u.DisplayName, StringComparer.CurrentCultureIgnoreCase)];
    }

    public Task SetGlobalAdminAsync(string uid, bool isGlobalAdmin, CancellationToken ct = default) =>
        interop.UpdateDocumentAsync($"users/{uid}", new
        {
            isGlobalAdmin,
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);
}
