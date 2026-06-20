using System.Text.Json.Serialization;

namespace OpenSync.Infrastructure.Realtime.Protocol;

public class InboundMessage
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("object_type")]
    public string? ObjectType { get; set; }

    [JsonPropertyName("object_id")]
    public string? ObjectId { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("data")]
    public System.Text.Json.JsonElement? Data { get; set; }

    [JsonPropertyName("metadata")]
    public System.Text.Json.JsonElement? Metadata { get; set; }

    [JsonPropertyName("expected_revision")]
    public long? ExpectedRevision { get; set; }
}
