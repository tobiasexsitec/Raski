using System.Text.Json.Serialization;

namespace Raski.Services.Firebase;

/// <summary>
/// Public snapshot payload for the shopping state listener. [JSInvokable]
/// methods require public signatures, so the internal document cannot be used.
/// </summary>
public sealed class ShoppingStatePayload
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("checked")] public bool Checked { get; set; }
    [JsonPropertyName("checkedByUid")] public string? CheckedByUid { get; set; }
    [JsonPropertyName("carId")] public string? CarId { get; set; }
}

/// <summary>Public snapshot payload for the car listener; see <see cref="ShoppingStatePayload"/>.</summary>
public sealed class CarPayload
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("memberUids")] public List<string>? MemberUids { get; set; }
}
