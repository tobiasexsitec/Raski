using System.Text.Json.Serialization;

namespace Raski.Services.Firebase;

/// <summary>
/// Public Firebase web configuration. These values are not secrets; access is
/// controlled by Firestore security rules. Bound from wwwroot/appsettings.json.
/// </summary>
public sealed class FirebaseOptions
{
    public const string SectionName = "Firebase";

    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = "";

    [JsonPropertyName("authDomain")]
    public string AuthDomain { get; set; } = "";

    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; } = "";

    [JsonPropertyName("storageBucket")]
    public string StorageBucket { get; set; } = "";

    [JsonPropertyName("messagingSenderId")]
    public string MessagingSenderId { get; set; } = "";

    [JsonPropertyName("appId")]
    public string AppId { get; set; } = "";

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ApiKey) && !ApiKey.StartsWith("REPLACE_WITH", StringComparison.Ordinal);
}
