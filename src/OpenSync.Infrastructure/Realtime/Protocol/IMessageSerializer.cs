namespace OpenSync.Infrastructure.Realtime.Protocol;

public interface IMessageSerializer
{
    string Serialize(object message);
    InboundMessage? Deserialize(string json);
    string SerializeEvent(string eventType, string objectType, string objectId, object? data = null);
}
