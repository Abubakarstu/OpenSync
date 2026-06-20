namespace OpenSync.Infrastructure.Realtime.Transports;

public interface ITransport
{
    Task SendAsync(string connectionId, string message, CancellationToken cancellationToken = default);
    Task SendManyAsync(IEnumerable<string> connectionIds, string message, CancellationToken cancellationToken = default);
    Task BroadcastAsync(string message, CancellationToken cancellationToken = default);
}
