using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace OpenSync.Infrastructure.Realtime.Transports;

public class SseTransport : ITransport
{
    private readonly ConcurrentDictionary<string, HttpResponse> _clients = new();

    public void AddClient(string connectionId, HttpResponse response)
    {
        _clients[connectionId] = response;
    }

    public void RemoveClient(string connectionId)
    {
        _clients.TryRemove(connectionId, out _);
    }

    public async Task SendAsync(string connectionId, string message, CancellationToken cancellationToken = default)
    {
        if (_clients.TryGetValue(connectionId, out var response))
        {
            await response.WriteAsync($"data: {message}\n\n", cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }
    }

    public async Task SendManyAsync(IEnumerable<string> connectionIds, string message, CancellationToken cancellationToken = default)
    {
        var tasks = connectionIds.Select(id => SendAsync(id, message, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public async Task BroadcastAsync(string message, CancellationToken cancellationToken = default)
    {
        await SendManyAsync(_clients.Keys.ToList(), message, cancellationToken);
    }
}
