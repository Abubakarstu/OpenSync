using OpenSync.Core.Interfaces.Services;
using OpenSync.Infrastructure.Realtime.Transports;

namespace OpenSync.Infrastructure.Realtime;

public class RealtimeNotifier : IRealtimeNotifier
{
    private readonly ITransport _transport;

    public RealtimeNotifier(ITransport transport)
    {
        _transport = transport;
    }

    public Task NotifyAsync(string connectionId, string message, CancellationToken cancellationToken = default)
        => _transport.SendAsync(connectionId, message, cancellationToken);

    public Task NotifyManyAsync(IEnumerable<string> connectionIds, string message, CancellationToken cancellationToken = default)
        => _transport.SendManyAsync(connectionIds, message, cancellationToken);

    public Task BroadcastAsync(string message, CancellationToken cancellationToken = default)
        => _transport.BroadcastAsync(message, cancellationToken);
}
