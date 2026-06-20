using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class StreamMessageEvent : BaseDomainEvent
{
    public string StreamId { get; }
    public string? Data { get; }
    public string? PublisherId { get; }

    public StreamMessageEvent(string streamId, string? data = null, string? publisherId = null)
        : base("stream_message")
    {
        StreamId = streamId;
        Data = data;
        PublisherId = publisherId;
    }
}
