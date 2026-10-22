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

    /// <summary>
    /// Onboarding revision the user has completed. Bump <see cref="CurrentOnboardingVersion"/>
    /// when a step is added so existing users are asked the new questions.
    /// </summary>
    public int OnboardingVersion { get; set; } = CurrentOnboardingVersion;

    /// <summary>2: partner and children (B-047).</summary>
    public const int CurrentOnboardingVersion = 2;

    /// <summary>Not persisted: the user chose "Senare" this session, so new steps are asked again at next sign-in.</summary>
    public bool OnboardingPostponed { get; set; }

    public bool NeedsOnboarding => !OnboardingCompleted || (OnboardingVersion < CurrentOnboardingVersion && !OnboardingPostponed);

    /// <summary>App version whose news the user has seen; null if never recorded.</summary>
    public string? LastSeenVersion { get; set; }
}

public static class ThemeOptions
{
    public const string Light = "light";
    public const string Dark = "dark";
    public const string System = "system";

    public static bool IsValid(string? theme) =>
        theme is Light or Dark or System;
}
