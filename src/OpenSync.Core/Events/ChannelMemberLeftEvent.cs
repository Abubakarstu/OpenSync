namespace OpenSync.Core.Events;

public class ChannelMemberLeftEvent : BaseDomainEvent
{
    public string ChannelId { get; }
    public string Identity { get; }

    public ChannelMemberLeftEvent(string channelId, string identity)
        : base("member_left")
    {
        ChannelId = channelId;
        Identity = identity;
    }
}
