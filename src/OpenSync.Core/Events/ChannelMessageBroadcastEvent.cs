namespace OpenSync.Core.Events;

public class ChannelMessageBroadcastEvent : BaseDomainEvent
{
    public string ChannelId { get; }
    public string? Data { get; }
    public string? PublisherId { get; }

    public ChannelMessageBroadcastEvent(string channelId, string? data = null, string? publisherId = null)
        : base("message_broadcast")
    {
        ChannelId = channelId;
        Data = data;
        PublisherId = publisherId;
    }
}
