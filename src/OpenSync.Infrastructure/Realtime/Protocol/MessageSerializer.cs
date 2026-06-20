using System.Text.Json;

namespace OpenSync.Infrastructure.Realtime.Protocol;

public class MessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public string Serialize(object message)
        => JsonSerializer.Serialize(message, _options);

    public InboundMessage? Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<InboundMessage>(json, _options);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public string SerializeEvent(string eventType, string objectType, string objectId, object? data = null)
        => Serialize(OutboundMessage.Create(eventType, objectType, objectId, data));
}
