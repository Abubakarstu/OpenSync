namespace OpenSync.Infrastructure.Realtime.Backplane;

public interface IBackplane
{
    Task PublishAsync(string channel, string message, CancellationToken cancellationToken = default);
    Task SubscribeAsync(string channel, Func<string, Task> handler, CancellationToken cancellationToken = default);
    Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default);
}
