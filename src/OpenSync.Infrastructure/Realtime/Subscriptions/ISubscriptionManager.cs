using OpenSync.Core.Enums;

namespace OpenSync.Infrastructure.Realtime.Subscriptions;

public interface ISubscriptionManager
{
    bool AddSubscription(string connectionId, SyncObjectType objectType, string objectId);
    bool RemoveSubscription(string connectionId, SyncObjectType objectType, string objectId);
    bool RemoveAllSubscriptions(string connectionId);
    IReadOnlyList<SubscriptionEntry> GetSubscriptions(string connectionId);
    IReadOnlyList<SubscriptionEntry> GetSubscribers(SyncObjectType objectType, string objectId);
    int GetSubscriptionCount(string connectionId);
    int TotalSubscriptions { get; }
}
