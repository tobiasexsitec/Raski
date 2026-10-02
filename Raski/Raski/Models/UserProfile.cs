namespace Raski.Models;

public sealed class UserProfile
{
    public string Uid { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhotoUrl { get; set; } = "";
    public string Theme { get; set; } = ThemeOptions.System;
    public string Phone { get; set; } = "";

    /// <summary>Global admins may create trips and promote other global admins.</summary>
    public bool IsGlobalAdmin { get; set; }

    /// <summary>Allergies and dietary preferences, stored normalized (trimmed, lowercase).</summary>
    public List<string> Allergies { get; set; } = [];

    /// <summary>Default for new trips; null for users who have not answered yet.</summary>
    public AlcoholPreference? Alcohol { get; set; }

    /// <summary>New users are sent through onboarding until this is set.</summary>
    public bool OnboardingCompleted { get; set; } = true;
}

public static class ThemeOptions
{
    public const string Light = "light";
    public const string Dark = "dark";
    public const string System = "system";

    public static bool IsValid(string? theme) =>
        theme is Light or Dark or System;
}
