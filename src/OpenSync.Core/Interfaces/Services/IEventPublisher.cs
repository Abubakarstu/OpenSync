using OpenSync.Core.Events;

namespace OpenSync.Core.Interfaces.Services;

public interface IEventPublisher
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : ISyncDomainEvent;
}
