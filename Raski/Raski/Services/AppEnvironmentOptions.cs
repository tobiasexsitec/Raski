namespace Raski.Services;

/// <summary>
/// Describes which deployment the app is running in. Empty in production; the test
/// pipeline swaps in an appsettings.json where <see cref="Name"/> is set.
/// </summary>
public sealed class AppEnvironmentOptions
{
    public const string SectionName = "AppEnvironment";

    public string Name { get; set; } = "";

    public bool IsProduction => string.IsNullOrWhiteSpace(Name);
}
