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

    public async Task SaveMyAlcoholAsync(AlcoholPreference preference, CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Ingen inloggad användare.");

        await interop.UpdateDocumentAsync($"users/{uid}", new
        {
            alcohol = preference.ToFirestore(),
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);

        authService.UpdateCachedProfile(p => p.Alcohol = preference);
    }

    public async Task CompleteOnboardingAsync(CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Ingen inloggad användare.");

        // New users have nothing to catch up on, so the news popup starts from the current version.
        // Existing users repeating new onboarding steps keep their news history.
        var isNewUser = authService.Current?.OnboardingCompleted != true;
        var version = isNewUser ? AppInfo.Version : authService.Current?.LastSeenVersion;

        var data = new Dictionary<string, object?>
        {
            ["onboardingCompleted"] = true,
            ["onboardingVersion"] = UserProfile.CurrentOnboardingVersion,
            ["updatedAt"] = FirestoreFormat.UtcNow()
        };

        if (isNewUser)
        {
            data["lastSeenVersion"] = version;
        }

        await interop.UpdateDocumentAsync($"users/{uid}", data, ct);

        authService.UpdateCachedProfile(p =>
        {
            p.OnboardingCompleted = true;
            p.OnboardingVersion = UserProfile.CurrentOnboardingVersion;
            p.LastSeenVersion = version;
        });
    }

    public async Task SaveMyLastSeenVersionAsync(string version, CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Ingen inloggad användare.");

        await interop.UpdateDocumentAsync($"users/{uid}", new
        {
            lastSeenVersion = version,
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);

        authService.UpdateCachedProfile(p => p.LastSeenVersion = version);
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
