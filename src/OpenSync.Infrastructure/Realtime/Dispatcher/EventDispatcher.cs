using OpenSync.Core.Events;
using OpenSync.Infrastructure.Realtime.Protocol;
using OpenSync.Infrastructure.Realtime.Subscriptions;
using Microsoft.Extensions.Logging;

namespace OpenSync.Infrastructure.Realtime.Dispatcher;

public class EventDispatcher : IEventDispatcher
{
    private readonly ISubscriptionManager _subscriptionManager;
    private readonly IMessageSerializer _serializer;
    private readonly FanOutStrategy _fanOut;
    private readonly ILogger<EventDispatcher> _logger;

    public EventDispatcher(
        ISubscriptionManager subscriptionManager,
        IMessageSerializer serializer,
        FanOutStrategy fanOut,
        ILogger<EventDispatcher> logger)
    {
        _subscriptionManager = subscriptionManager;
        _serializer = serializer;
        _fanOut = fanOut;
        _logger = logger;
    }

    public async Task DispatchAsync(ISyncDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var (objectType, objectId, eventType, data) = ResolveEvent(domainEvent);
        if (objectType == null || objectId == null) return;

        var subscribers = _subscriptionManager.GetSubscribers(
            Enum.Parse<Core.Enums.SyncObjectType>(objectType, true), objectId);

        if (subscribers.Count == 0) return;

        var message = _serializer.SerializeEvent(eventType, objectType, objectId, data);
        var connectionIds = subscribers.Select(s => s.ConnectionId).Distinct().ToList();

        _logger.LogDebug("Dispatching {EventType} for {ObjectType}/{ObjectId} to {Count} connections",
            eventType, objectType, objectId, connectionIds.Count);

        await _fanOut.SendAsync(connectionIds, message, cancellationToken);
    }

    private static (string? objectType, string? objectId, string eventType, object? data) ResolveEvent(ISyncDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            ObjectCreatedEvent e => (e.ObjectType.ToString().ToLower(), e.ObjectId, ProtocolConstants.EventObjectCreated, e.Data),
            ObjectUpdatedEvent e => (e.ObjectType.ToString().ToLower(), e.ObjectId, ProtocolConstants.EventObjectUpdated, new { data = e.Data, revision = e.Revision }),
            ObjectRemovedEvent e => (e.ObjectType.ToString().ToLower(), e.ObjectId, ProtocolConstants.EventObjectRemoved, null),
            ItemAddedEvent e => (e.ObjectType.ToString().ToLower(), e.ObjectId, ProtocolConstants.EventItemAdded, new { item_id = e.ItemId, index = e.Index, data = e.Data }),
            ItemUpdatedEvent e => (e.ObjectType.ToString().ToLower(), e.ObjectId, ProtocolConstants.EventItemUpdated, new { item_id = e.ItemId, data = e.Data, revision = e.Revision }),
            ItemRemovedEvent e => (e.ObjectType.ToString().ToLower(), e.ObjectId, ProtocolConstants.EventItemRemoved, new { item_id = e.ItemId, index = e.Index }),
            StreamMessageEvent e => ("stream", e.StreamId, ProtocolConstants.EventStreamMessage, e.Data),
            ChannelMemberJoinedEvent e => ("channel", e.ChannelId, ProtocolConstants.EventMemberJoined, new { identity = e.Identity, metadata = e.Metadata }),
            ChannelMemberLeftEvent e => ("channel", e.ChannelId, ProtocolConstants.EventMemberLeft, new { identity = e.Identity }),
            ChannelPresenceUpdatedEvent e => ("channel", e.ChannelId, ProtocolConstants.EventPresenceUpdated, new { identity = e.Identity, metadata = e.Metadata }),
            ChannelMessageBroadcastEvent e => ("channel", e.ChannelId, ProtocolConstants.EventMessageBroadcast, new { data = e.Data, publisher = e.PublisherId }),
            _ => (null, null, string.Empty, null)
        };
    }
}
