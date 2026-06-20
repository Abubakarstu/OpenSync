using System.Text.Json;

namespace OpenSync.Core.ValueObjects;

public class JsonData : IEquatable<JsonData>
{
    public string Raw { get; }
    public long SizeBytes { get; }

    private static readonly JsonDocumentOptions _parseOptions = new()
    {
        AllowTrailingCommas = false,
        MaxDepth = 32
    };

    public JsonData(string raw, long? maxSizeBytes = null)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("JSON data cannot be empty", nameof(raw));

        if (maxSizeBytes.HasValue && raw.Length > maxSizeBytes.Value)
            throw new Exceptions.PayloadTooLargeException(raw.Length, maxSizeBytes.Value);

        try
        {
            using var doc = JsonDocument.Parse(raw, _parseOptions);
        }
        catch (JsonException ex)
        {
            throw new ArgumentException($"Invalid JSON: {ex.Message}", nameof(raw));
        }

        Raw = raw;
        SizeBytes = raw.Length;
    }

    public JsonElement ToJsonElement()
    {
        using var doc = JsonDocument.Parse(Raw);
        return doc.RootElement.Clone();
    }

    public T? Deserialize<T>(JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(Raw, options);
    }

    public static JsonData FromObject<T>(T value, JsonSerializerOptions? options = null, long? maxSizeBytes = null)
    {
        var json = JsonSerializer.Serialize(value, options);
        return new JsonData(json, maxSizeBytes);
    }

    public bool Equals(JsonData? other)
    {
        if (other is null) return false;
        return Raw == other.Raw;
    }

    public override bool Equals(object? obj) => obj is JsonData other && Equals(other);
    public override int GetHashCode() => Raw.GetHashCode();
    public override string ToString() => Raw;
}
