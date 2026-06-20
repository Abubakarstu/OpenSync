namespace OpenSync.Infrastructure.Realtime.Protocol;

public static class ProtocolConstants
{
    public const string ActionSubscribe = "subscribe";
    public const string ActionUnsubscribe = "unsubscribe";
    public const string ActionPresence = "presence";
    public const string ActionChannelMessage = "channel_message";
    public const string ActionPong = "pong";

    public const string EventObjectCreated = "object_created";
    public const string EventObjectUpdated = "object_updated";
    public const string EventObjectRemoved = "object_removed";
    public const string EventItemAdded = "item_added";
    public const string EventItemUpdated = "item_updated";
    public const string EventItemRemoved = "item_removed";
    public const string EventStreamMessage = "stream_message";
    public const string EventMemberJoined = "member_joined";
    public const string EventMemberLeft = "member_left";
    public const string EventPresenceUpdated = "presence_updated";
    public const string EventMessageBroadcast = "message_broadcast";
    public const string EventPing = "ping";
    public const string EventError = "error";

    public const string ObjectTypeDocument = "document";
    public const string ObjectTypeList = "list";
    public const string ObjectTypeMap = "map";
    public const string ObjectTypeStream = "stream";
    public const string ObjectTypeChannel = "channel";
}
