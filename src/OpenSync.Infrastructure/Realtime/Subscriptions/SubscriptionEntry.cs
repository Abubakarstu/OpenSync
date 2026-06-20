using OpenSync.Core.Enums;

namespace OpenSync.Infrastructure.Realtime.Subscriptions;

public class SubscriptionEntry
{
    public string ConnectionId { get; }
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }
    public DateTime SubscribedAt { get; }

    public SubscriptionEntry(string connectionId, SyncObjectType objectType, string objectId)
    {
        ConnectionId = connectionId;
        ObjectType = objectType;
        ObjectId = objectId;
        SubscribedAt = DateTime.UtcNow;
    }
}
