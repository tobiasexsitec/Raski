namespace Raski.Models;

public sealed class UserProfile
{
    public string Uid { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhotoUrl { get; set; } = "";
    public string Theme { get; set; } = ThemeOptions.System;
}

public static class ThemeOptions
{
    public const string Light = "light";
    public const string Dark = "dark";
    public const string System = "system";

    public static bool IsValid(string? theme) =>
        theme is Light or Dark or System;
}
