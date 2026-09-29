using Raski.Models;

namespace Raski.Services;

public interface IAuthService
{
    UserProfile? Current { get; }

    /// <summary>True once the initial auth state has been resolved.</summary>
    bool IsInitialized { get; }

    event Action? StateChanged;

    Task InitializeAsync(CancellationToken ct = default);
    Task SignInWithGoogleAsync();
    Task SignOutAsync();

    /// <summary>Updates the cached profile after a local change such as a theme switch.</summary>
    void UpdateCachedProfile(Action<UserProfile> update);
}
