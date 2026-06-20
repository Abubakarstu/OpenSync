namespace OpenSync.Core.Events;

public class ChannelMemberJoinedEvent : BaseDomainEvent
{
    public string ChannelId { get; }
    public string Identity { get; }
    public string? Metadata { get; }

    public ChannelMemberJoinedEvent(string channelId, string identity, string? metadata = null)
        : base("member_joined")
    {
        ChannelId = channelId;
        Identity = identity;
        Metadata = metadata;
    }
}
