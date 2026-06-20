using System.Text.Json.Serialization;

namespace OpenSync.Infrastructure.Realtime.Protocol;

public class OutboundMessage
{
    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;

    [JsonPropertyName("object_type")]
    public string ObjectType { get; set; } = string.Empty;

    [JsonPropertyName("object_id")]
    public string ObjectId { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public object? Data { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static OutboundMessage Create(string eventType, string objectType, string objectId, object? data = null)
        => new()
        {
            Event = eventType,
            ObjectType = objectType,
            ObjectId = objectId,
            Data = data,
            Timestamp = DateTime.UtcNow
        };
}
