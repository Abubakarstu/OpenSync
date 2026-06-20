using System.Text.Json;
using OpenSync.Sdk.Events;

namespace OpenSync.Sdk.Serialization;

public class JsonEventSerializer
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public string Serialize(object obj)
        => JsonSerializer.Serialize(obj, _options);

    public T Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, _options)!;

    public SyncEventArgs? DeserializeEvent(string json)
    {
        try
        {
            var doc = JsonDocument.Parse(json);
            return new SyncEventArgs
            {
                Event = doc.RootElement.GetProperty("event").GetString() ?? string.Empty,
                ObjectType = doc.RootElement.GetProperty("object_type").GetString() ?? string.Empty,
                ObjectId = doc.RootElement.GetProperty("object_id").GetString() ?? string.Empty,
                Data = doc.RootElement.TryGetProperty("data", out var data) ? data.GetRawText() : null,
                Timestamp = doc.RootElement.TryGetProperty("timestamp", out var ts) ? ts.GetDateTime() : DateTime.UtcNow
            };
        }
        catch
        {
            return null;
        }
    }
}
