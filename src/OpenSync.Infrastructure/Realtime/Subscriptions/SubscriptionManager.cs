using OpenSync.Core.Enums;
using System.Collections.Concurrent;

namespace OpenSync.Infrastructure.Realtime.Subscriptions;

public class SubscriptionManager : ISubscriptionManager
{
    private readonly ConcurrentDictionary<string, List<SubscriptionEntry>> _subscriptions = new();
    private readonly ConcurrentDictionary<(SyncObjectType, string), HashSet<string>> _invertedIndex = new();

    public bool AddSubscription(string connectionId, SyncObjectType objectType, string objectId)
    {
        var entry = new SubscriptionEntry(connectionId, objectType, objectId);

        _subscriptions.AddOrUpdate(
            connectionId,
            _ => new List<SubscriptionEntry> { entry },
            (_, list) => { list.Add(entry); return list; });

        _invertedIndex.AddOrUpdate(
            (objectType, objectId),
            _ => new HashSet<string> { connectionId },
            (_, set) => { set.Add(connectionId); return set; });

        return true;
    }

    public bool RemoveSubscription(string connectionId, SyncObjectType objectType, string objectId)
    {
        if (_subscriptions.TryGetValue(connectionId, out var list))
        {
            list.RemoveAll(s => s.ObjectType == objectType && s.ObjectId == objectId);
            if (list.Count == 0)
                _subscriptions.TryRemove(connectionId, out _);
        }

        if (_invertedIndex.TryGetValue((objectType, objectId), out var set))
        {
            set.Remove(connectionId);
            if (set.Count == 0)
                _invertedIndex.TryRemove((objectType, objectId), out _);
        }

        return true;
    }

    public bool RemoveAllSubscriptions(string connectionId)
    {
        if (_subscriptions.TryRemove(connectionId, out var entries))
        {
            foreach (var entry in entries)
            {
                if (_invertedIndex.TryGetValue((entry.ObjectType, entry.ObjectId), out var set))
                {
                    set.Remove(connectionId);
                    if (set.Count == 0)
                        _invertedIndex.TryRemove((entry.ObjectType, entry.ObjectId), out _);
                }
            }
            return true;
        }
        return false;
    }

    public IReadOnlyList<SubscriptionEntry> GetSubscriptions(string connectionId)
    {
        if (_subscriptions.TryGetValue(connectionId, out var list))
            return list.AsReadOnly();
        return Array.Empty<SubscriptionEntry>();
    }

    public IReadOnlyList<SubscriptionEntry> GetSubscribers(SyncObjectType objectType, string objectId)
    {
        if (_invertedIndex.TryGetValue((objectType, objectId), out var connectionIds))
        {
            return connectionIds
                .SelectMany(cid => _subscriptions.GetValueOrDefault(cid) ?? new())
                .Where(s => s.ObjectType == objectType && s.ObjectId == objectId)
                .ToList()
                .AsReadOnly();
        }
        return Array.Empty<SubscriptionEntry>();
    }

    public int GetSubscriptionCount(string connectionId)
    {
        if (_subscriptions.TryGetValue(connectionId, out var list))
            return list.Count;
        return 0;
    }

    public int TotalSubscriptions =>
        _subscriptions.Values.Sum(list => list.Count);
}
