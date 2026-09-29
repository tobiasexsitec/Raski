namespace Raski.Services;

public interface IThemeService
{
    /// <summary>The user's choice: "light", "dark" or "system".</summary>
    string Theme { get; }

    /// <summary>The theme actually applied: "light" or "dark".</summary>
    string ResolvedTheme { get; }

    event Action? Changed;

    Task InitializeAsync(CancellationToken ct = default);
    Task SetThemeAsync(string theme);
}
