using Microsoft.JSInterop;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

/// <summary>
/// Keeps the theme in three places: the data-theme attribute for immediate effect,
/// localStorage so the choice survives a reload before auth resolves, and
/// users/{uid}.theme so it follows the user across devices.
/// </summary>
public sealed class ThemeService(IJSRuntime jsRuntime, IAuthService authService, FirebaseInterop interop)
    : IThemeService, IAsyncDisposable
{
    private IJSObjectReference? _module;
    private IJSObjectReference? _systemListener;
    private DotNetObjectReference<ThemeService>? _selfRef;
    private bool _initialized;
    private string? _syncedUid;

    public string Theme { get; private set; } = ThemeOptions.System;

    public string ResolvedTheme { get; private set; } = ThemeOptions.Light;

    public event Action? Changed;

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;
        _module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", ct, "./js/theme-interop.js");

        var stored = await _module.InvokeAsync<string?>("getStoredTheme", ct);
        Theme = ThemeOptions.IsValid(stored) ? stored! : ThemeOptions.System;

        _selfRef = DotNetObjectReference.Create(this);
        _systemListener = await _module.InvokeAsync<IJSObjectReference>(
            "listenToSystemTheme", ct, _selfRef, nameof(OnSystemThemeChangedAsync));

        await ApplyAsync(Theme);

        authService.StateChanged += OnAuthStateChanged;
        OnAuthStateChanged();
    }

    /// <summary>The stored preference wins over localStorage once the user is known.</summary>
    private void OnAuthStateChanged()
    {
        var profile = authService.Current;

        if (profile is null || profile.Uid == _syncedUid)
        {
            return;
        }

        _syncedUid = profile.Uid;

        if (profile.Theme != Theme)
        {
            _ = ApplyAsync(profile.Theme);
        }
    }

    public async Task SetThemeAsync(string theme)
    {
        if (!ThemeOptions.IsValid(theme))
        {
            return;
        }

        await ApplyAsync(theme);

        var profile = authService.Current;

        if (profile is null)
        {
            return;
        }

        authService.UpdateCachedProfile(p => p.Theme = theme);

        try
        {
            await interop.SetDocumentAsync($"users/{profile.Uid}", new { theme });
        }
        catch (JSException)
        {
            // The local theme already applied; syncing can retry on the next change.
        }
    }

    private async Task ApplyAsync(string theme)
    {
        Theme = theme;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("applyTheme", theme);
            ResolvedTheme = await _module.InvokeAsync<string>("getResolvedTheme", theme);
        }

        Changed?.Invoke();
    }

    [JSInvokable]
    public async Task OnSystemThemeChangedAsync()
    {
        if (Theme == ThemeOptions.System)
        {
            await ApplyAsync(ThemeOptions.System);
        }
    }

    public async ValueTask DisposeAsync()
    {
        authService.StateChanged -= OnAuthStateChanged;

        try
        {
            if (_systemListener is not null)
            {
                await _systemListener.InvokeVoidAsync("dispose");
                await _systemListener.DisposeAsync();
            }

            if (_module is not null)
            {
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
        }

        _selfRef?.Dispose();
    }
}
