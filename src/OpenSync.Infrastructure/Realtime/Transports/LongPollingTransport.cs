using System.Collections.Concurrent;

namespace OpenSync.Infrastructure.Realtime.Transports;

public class LongPollingTransport : ITransport
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pending = new();

    public TaskCompletionSource<string> CreatePoll(string connectionId)
    {
        var tcs = new TaskCompletionSource<string>();
        _pending[connectionId] = tcs;
        return tcs;
    }

    public Task SendAsync(string connectionId, string message, CancellationToken cancellationToken = default)
    {
        if (_pending.TryRemove(connectionId, out var tcs))
        {
            tcs.TrySetResult(message);
        }

        return Task.CompletedTask;
    }

    public async Task SendManyAsync(IEnumerable<string> connectionIds, string message, CancellationToken cancellationToken = default)
    {
        var tasks = connectionIds.Select(id => SendAsync(id, message, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public Task BroadcastAsync(string message, CancellationToken cancellationToken = default)
    {
        foreach (var kvp in _pending)
        {
            if (_pending.TryRemove(kvp.Key, out var tcs))
            {
                tcs.TrySetResult(message);
            }
        }

        return Task.CompletedTask;
    }
}
