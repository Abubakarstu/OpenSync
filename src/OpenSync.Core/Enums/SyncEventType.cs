namespace OpenSync.Core.Enums;

public enum SyncEventType
{
    ObjectCreated,
    ObjectUpdated,
    ObjectRemoved,
    ItemAdded,
    ItemUpdated,
    ItemRemoved,
    StreamMessagePublished,
    ChannelMemberJoined,
    ChannelMemberLeft,
    ChannelPresenceUpdated,
    ChannelMessageBroadcast,
    SubscriptionAdded,
    SubscriptionRemoved,
    ConnectionStateChanged
}
