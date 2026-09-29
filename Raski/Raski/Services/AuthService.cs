using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class AuthService(FirebaseInterop interop) : IAuthService, IAsyncDisposable
{
    private DotNetObjectReference<AuthService>? _selfRef;
    private string? _listenerHandle;
    private bool _initialized;

    public UserProfile? Current { get; private set; }

    public bool IsInitialized { get; private set; }

    public event Action? StateChanged;

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;
        _selfRef = DotNetObjectReference.Create(this);

        // Pick up a pending redirect sign-in before subscribing to state changes.
        try
        {
            await interop.InvokeAsync<FirebaseUser?>("completeRedirectSignIn", ct);
        }
        catch (JSException)
        {
            // No pending redirect, or the provider rejected it. The state listener decides.
        }

        _listenerHandle = await interop.InvokeAsync<string>(
            "listenToAuthState", ct, _selfRef, nameof(OnAuthStateChangedAsync));
    }

    [JSInvokable]
    public async Task OnAuthStateChangedAsync(FirebaseUser? user)
    {
        if (user is null)
        {
            Current = null;
        }
        else
        {
            Current = await EnsureUserDocumentAsync(user);
            await ClaimInviteAsync(Current);
        }

        IsInitialized = true;
        StateChanged?.Invoke();
    }

    public Task SignInWithGoogleAsync() =>
        interop.InvokeAsync<FirebaseUser?>("signInWithGoogle", CancellationToken.None);

    public async Task SignOutAsync()
    {
        await interop.InvokeVoidAsync("signOut", CancellationToken.None);
        Current = null;
        StateChanged?.Invoke();
    }

    public void UpdateCachedProfile(Action<UserProfile> update)
    {
        if (Current is null)
        {
            return;
        }

        update(Current);
        StateChanged?.Invoke();
    }

    /// <summary>Creates users/{uid} on first sign-in and refreshes the denormalized profile fields.</summary>
    private async Task<UserProfile> EnsureUserDocumentAsync(FirebaseUser user)
    {
        var existing = await interop.GetDocumentAsync<UserDocument>($"users/{user.Uid}");

        var profile = new UserProfile
        {
            Uid = user.Uid,
            DisplayName = string.IsNullOrWhiteSpace(user.DisplayName) ? user.Email : user.DisplayName,
            Email = user.Email,
            PhotoUrl = user.PhotoUrl,
            Theme = ThemeOptions.IsValid(existing?.Theme) ? existing!.Theme! : ThemeOptions.System,
            // Only another global admin may set this flag, so it is read-only here.
            IsGlobalAdmin = existing?.IsGlobalAdmin ?? false,
            Allergies = AllergyNames.Distinct(existing?.Allergies)
        };

        // Built as a dictionary so createdAt is only written once; a null value
        // would otherwise clear the existing field on a merging write.
        var data = new Dictionary<string, object?>
        {
            ["displayName"] = profile.DisplayName,
            ["email"] = profile.Email,
            ["photoUrl"] = profile.PhotoUrl,
            ["theme"] = profile.Theme,
            ["updatedAt"] = FirestoreFormat.UtcNow()
        };

        if (existing is null)
        {
            data["createdAt"] = FirestoreFormat.UtcNow();
        }

        await interop.SetDocumentAsync($"users/{user.Uid}", data);

        return profile;
    }

    /// <summary>
    /// Firestore rules cannot resolve a uid from an email, so invitations are stored per
    /// email address under invites/{email}/trips/{tripId}. On first sign-in the invited
    /// user adds their own uid to the trip, which the rules permit while the invite exists.
    /// </summary>
    private async Task ClaimInviteAsync(UserProfile profile)
    {
        if (string.IsNullOrWhiteSpace(profile.Email))
        {
            return;
        }

        var emailKey = profile.Email.ToLowerInvariant();

        try
        {
            var invites = await interop.QueryAsync<InviteDocument>($"invites/{emailKey}/trips");

            foreach (var invite in invites.Where(i => !string.IsNullOrEmpty(i.TripId)))
            {
                // The membership document carries the denormalized profile used when rendering.
                await interop.SetDocumentAsync($"trips/{invite.TripId}/members/{profile.Uid}", new
                {
                    displayName = profile.DisplayName,
                    email = profile.Email,
                    photoUrl = profile.PhotoUrl,
                    role = MemberRoles.Member
                });

                // memberUids drives the "my trips" array-contains query and the security rules.
                await interop.AddToArrayAsync($"trips/{invite.TripId}", "memberUids", profile.Uid);

                await interop.DeleteDocumentAsync($"invites/{emailKey}/trips/{invite.Id}");
            }
        }
        catch (JSException)
        {
            // Invitations are best-effort; a rules rejection must not block sign-in.
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_listenerHandle is not null)
        {
            await interop.UnsubscribeAsync(_listenerHandle);
        }

        _selfRef?.Dispose();
    }
}

/// <summary>Auth payload as returned by firebase-interop.js.</summary>
public sealed class FirebaseUser
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("photoUrl")] public string PhotoUrl { get; set; } = "";
}
