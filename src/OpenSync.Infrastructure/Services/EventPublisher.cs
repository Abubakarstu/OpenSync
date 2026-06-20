using Microsoft.Extensions.Logging;
using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Infrastructure.Realtime.Dispatcher;

namespace OpenSync.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IEventDispatcher eventDispatcher, ILogger<EventPublisher> logger)
    {
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : ISyncDomainEvent
    {
        try
        {
            await _eventDispatcher.DispatchAsync(domainEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to dispatch domain event {EventType}", typeof(T).Name);
        }
    }
}
