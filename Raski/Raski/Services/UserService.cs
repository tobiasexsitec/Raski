using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class UserService(FirebaseInterop interop, IAuthService authService) : IUserService
{
    public async Task SaveMyPhoneAsync(string phone, CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Ingen inloggad användare.");
        var normalized = phone.Trim();

        await interop.UpdateDocumentAsync($"users/{uid}", new
        {
            phone = normalized,
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);

        authService.UpdateCachedProfile(p => p.Phone = normalized);
    }

    public async Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<UserDocument>("users", null, ct);

        return [.. documents
            .Select(d => d.ToModel())
            .OrderBy(u => u.DisplayName, StringComparer.CurrentCultureIgnoreCase)];
    }

    public async Task<UserProfile?> GetAsync(string uid, CancellationToken ct = default)
    {
        var document = await interop.GetDocumentAsync<UserDocument>($"users/{uid}", ct);
        return document?.ToModel();
    }

    public Task SetGlobalAdminAsync(string uid, bool isGlobalAdmin, CancellationToken ct = default) =>
        interop.UpdateDocumentAsync($"users/{uid}", new
        {
            isGlobalAdmin,
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);
}
