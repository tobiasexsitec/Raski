using System.Reflection;

namespace Raski.Services;

public static class AppInfo
{
    public static readonly string InformationalVersion =
        typeof(AppInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? typeof(AppInfo).Assembly.GetName().Version?.ToString()
        ?? "?";

    /// <summary>The release version without build metadata (e.g. "0.21.0" from "0.21.0+abc123").</summary>
    public static readonly string Version = InformationalVersion.Split('+')[0];

    public static Version? Parse(string? version) =>
        System.Version.TryParse(version?.Split('+')[0], out var parsed) ? parsed : null;
}
