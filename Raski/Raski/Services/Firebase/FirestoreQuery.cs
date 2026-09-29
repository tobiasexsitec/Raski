using System.Text.Json;
using System.Text.Json.Serialization;

namespace Raski.Services.Firebase;

/// <summary>Serializable description of a Firestore query, mirrored in firebase-interop.js.</summary>
public sealed class FirestoreQuery
{
    [JsonPropertyName("where")]
    public List<FirestoreFilter> Where { get; set; } = [];

    [JsonPropertyName("orderBy")]
    public string? OrderBy { get; set; }

    [JsonPropertyName("descending")]
    public bool Descending { get; set; }

    [JsonPropertyName("startAt")]
    public string? StartAt { get; set; }

    [JsonPropertyName("endAt")]
    public string? EndAt { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
}

public sealed class FirestoreFilter
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = "";

    [JsonPropertyName("op")]
    public string Op { get; set; } = "==";

    [JsonPropertyName("value")]
    public JsonElement Value { get; set; }

    public static FirestoreFilter ArrayContains(string field, string value) =>
        new() { Field = field, Op = "array-contains", Value = ToElement(value) };

    public static FirestoreFilter Equal(string field, string value) =>
        new() { Field = field, Op = "==", Value = ToElement(value) };

    private static JsonElement ToElement(string value) =>
        JsonSerializer.SerializeToElement(value);
}

/// <summary>A document write used by <see cref="FirebaseInterop.BatchSetAsync"/>.</summary>
public sealed class FirestoreWrite
{
    [JsonPropertyName("path")]
    public string Path { get; set; } = "";

    [JsonPropertyName("data")]
    public object Data { get; set; } = new();

    [JsonPropertyName("merge")]
    public bool Merge { get; set; } = true;
}
