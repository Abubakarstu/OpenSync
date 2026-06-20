using OpenSync.Core.Events;

namespace OpenSync.Infrastructure.Realtime.Dispatcher;

public interface IEventDispatcher
{
    Task DispatchAsync(ISyncDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
