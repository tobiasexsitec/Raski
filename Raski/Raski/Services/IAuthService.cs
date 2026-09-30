using Raski.Models;

namespace Raski.Services;

public interface IAuthService
{
    UserProfile? Current { get; }

    /// <summary>True once the initial auth state has been resolved.</summary>
    bool IsInitialized { get; }

    /// <summary>True when the signed in user may create trips and manage other global admins.</summary>
    bool IsGlobalAdmin => Current?.IsGlobalAdmin == true;

    event Action? StateChanged;

    Task InitializeAsync(CancellationToken ct = default);
    Task SignInWithGoogleAsync();

    /// <summary>Renders the Google Identity Services button; errors are reported to <paramref name="callbackTarget"/>.</summary>
    Task RenderGoogleButtonAsync(Microsoft.AspNetCore.Components.ElementReference element, object callbackTarget);
    Task SignOutAsync();

    /// <summary>Updates the cached profile after a local change such as a theme switch.</summary>
    void UpdateCachedProfile(Action<UserProfile> update);
}
