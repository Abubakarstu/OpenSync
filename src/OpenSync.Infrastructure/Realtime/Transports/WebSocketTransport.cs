using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenSync.Infrastructure.Realtime.Connection;
using OpenSync.Infrastructure.Realtime.Subscriptions;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace OpenSync.Infrastructure.Realtime.Transports;

public class WebSocketTransport : ITransport
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();
    private readonly ILogger<WebSocketTransport> _logger;

    public WebSocketTransport(ILogger<WebSocketTransport> logger)
    {
        _logger = logger;
    }

    public void AddSocket(string connectionId, WebSocket socket)
    {
        _sockets[connectionId] = socket;
    }

    public void RemoveSocket(string connectionId)
    {
        _sockets.TryRemove(connectionId, out _);
    }

    public async Task SendAsync(string connectionId, string message, CancellationToken cancellationToken = default)
    {
        if (_sockets.TryGetValue(connectionId, out var socket) && socket.State == WebSocketState.Open)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }
    }

    public async Task SendManyAsync(IEnumerable<string> connectionIds, string message, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(bytes);
        var tasks = new List<Task>();

        foreach (var connectionId in connectionIds)
        {
            if (_sockets.TryGetValue(connectionId, out var socket) && socket.State == WebSocketState.Open)
            {
                tasks.Add(socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken));
            }
        }

        await Task.WhenAll(tasks);
    }

    public async Task BroadcastAsync(string message, CancellationToken cancellationToken = default)
    {
        await SendManyAsync(_sockets.Keys.ToList(), message, cancellationToken);
    }
}
