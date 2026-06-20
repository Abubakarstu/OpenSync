using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenSync.Infrastructure.Realtime.Protocol;
using OpenSync.Infrastructure.Realtime.Connection;
using OpenSync.Infrastructure.Realtime.Subscriptions;
using OpenSync.Core.Enums;
using OpenSync.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;

namespace OpenSync.AspNetCore.Middleware;

public class SyncWebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SyncWebSocketMiddleware> _logger;

    public SyncWebSocketMiddleware(RequestDelegate next, ILogger<SyncWebSocketMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
        {
            var ws = await context.WebSockets.AcceptWebSocketAsync();
            var identity = context.User?.Identity?.Name ?? "anonymous";
            var connectionManager = context.RequestServices.GetRequiredService<IConnectionManager>();
            var subscriptionManager = context.RequestServices.GetRequiredService<ISubscriptionManager>();
            var serializer = context.RequestServices.GetRequiredService<IMessageSerializer>();

            var connectionId = connectionManager.AddConnection(identity, TransportType.WebSocket);
            _logger.LogInformation("WebSocket connected: {ConnectionId} for {Identity}", connectionId, identity);

            var buffer = new byte[1024 * 16];
            try
            {
                while (ws.State == WebSocketState.Open)
                {
                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        break;
                    }

                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var message = serializer.Deserialize(json);
                    if (message == null) continue;

                    switch (message.Action)
                    {
                        case "subscribe":
                            if (Enum.TryParse<SyncObjectType>(message.ObjectType, true, out var objType))
                                subscriptionManager.AddSubscription(connectionId, objType, message.ObjectId!);
                            break;
                        case "unsubscribe":
                            if (Enum.TryParse<SyncObjectType>(message.ObjectType, true, out var unsubType))
                                subscriptionManager.RemoveSubscription(connectionId, unsubType, message.ObjectId!);
                            break;
                        case "pong":
                            connectionManager.UpdateHeartbeat(connectionId);
                            break;
                    }
                }
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning(ex, "WebSocket error for {ConnectionId}", connectionId);
            }
            finally
            {
                connectionManager.RemoveConnection(connectionId);
                subscriptionManager.RemoveAllSubscriptions(connectionId);
                _logger.LogInformation("WebSocket disconnected: {ConnectionId}", connectionId);
            }
        }
        else
        {
            await _next(context);
        }
    }
}
