namespace OpenSync.Core.Events;

public class ChannelPresenceUpdatedEvent : BaseDomainEvent
{
    public string ChannelId { get; }
    public string Identity { get; }
    public string? Metadata { get; }

    public ChannelPresenceUpdatedEvent(string channelId, string identity, string? metadata = null)
        : base("presence_updated")
    {
        ChannelId = channelId;
        Identity = identity;
        Metadata = metadata;
    }
}
