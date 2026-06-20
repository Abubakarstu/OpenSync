namespace OpenSync.Core.Interfaces.Services;

public interface IRealtimeNotifier
{
    Task NotifyAsync(string connectionId, string message, CancellationToken cancellationToken = default);
    Task NotifyManyAsync(IEnumerable<string> connectionIds, string message, CancellationToken cancellationToken = default);
    Task BroadcastAsync(string message, CancellationToken cancellationToken = default);
}
