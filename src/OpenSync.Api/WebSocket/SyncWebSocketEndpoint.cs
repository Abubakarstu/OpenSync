using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using OpenSync.Infrastructure.Realtime.Connection;
using OpenSync.Infrastructure.Realtime.Protocol;
using OpenSync.Infrastructure.Realtime.Subscriptions;
using OpenSync.Core.Enums;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Api.WebSockets;

public class SyncWebSocketEndpoint
{
    private readonly IConnectionManager _connectionManager;
    private readonly ISubscriptionManager _subscriptionManager;
    private readonly IMessageSerializer _messageSerializer;
    private readonly ITokenService _tokenService;
    private readonly ILogger<SyncWebSocketEndpoint> _logger;

    public SyncWebSocketEndpoint(
        IConnectionManager connectionManager,
        ISubscriptionManager subscriptionManager,
        IMessageSerializer messageSerializer,
        ITokenService tokenService,
        ILogger<SyncWebSocketEndpoint> logger)
    {
        _connectionManager = connectionManager;
        _subscriptionManager = subscriptionManager;
        _messageSerializer = messageSerializer;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        var tokenStr = context.Request.Query["token"].FirstOrDefault();
        if (string.IsNullOrEmpty(tokenStr))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"error\":\"Missing token parameter\"}");
            return;
        }

        var payload = _tokenService.ValidateToken(tokenStr, "default-secret-key-change-in-production");
        if (payload == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"error\":\"Invalid or expired token\"}");
            return;
        }

        var identity = payload.Identity;
        var transportType = TransportType.WebSocket;

        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var connectionId = _connectionManager.AddConnection(identity, transportType);

        _logger.LogInformation("WebSocket connected: {ConnectionId} for identity {Identity}", connectionId, identity);

        try
        {
            var buffer = new byte[1024 * 16];
            var messageBuffer = new StringBuilder();

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    messageBuffer.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                    if (result.EndOfMessage)
                    {
                        var messageText = messageBuffer.ToString();
                        messageBuffer.Clear();

                        _connectionManager.UpdateHeartbeat(connectionId);

                        var message = _messageSerializer.Deserialize(messageText);
                        if (message == null)
                        {
                            await SendErrorAsync(webSocket, "Invalid message format");
                            continue;
                        }

                        await ProcessMessageAsync(webSocket, connectionId, identity, message);
                    }
                }
            }
        }
        catch (WebSocketException ex)
        {
            _logger.LogWarning(ex, "WebSocket error for connection {ConnectionId}", connectionId);
        }
        finally
        {
            _connectionManager.RemoveConnection(connectionId);
            _subscriptionManager.RemoveAllSubscriptions(connectionId);

            if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
            {
                try
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
                }
                catch { }
            }

            _logger.LogInformation("WebSocket disconnected: {ConnectionId} for identity {Identity}", connectionId, identity);
        }
    }

    private async Task ProcessMessageAsync(WebSocket webSocket, string connectionId, string identity, InboundMessage message)
    {
        switch (message.Action)
        {
            case "subscribe":
                await HandleSubscribeAsync(webSocket, connectionId, message);
                break;
            case "unsubscribe":
                await HandleUnsubscribeAsync(webSocket, connectionId, message);
                break;
            case "ping":
                await SendEventAsync(webSocket, "pong", message.ObjectType ?? "system", message.ObjectId ?? connectionId);
                break;
            default:
                await SendErrorAsync(webSocket, $"Unknown action: {message.Action}");
                break;
        }
    }

    private async Task HandleSubscribeAsync(WebSocket webSocket, string connectionId, InboundMessage message)
    {
        var objectTypeStr = message.ObjectType;
        var objectId = message.ObjectId;

        if (string.IsNullOrEmpty(objectTypeStr) || string.IsNullOrEmpty(objectId))
        {
            await SendErrorAsync(webSocket, "Subscribe requires 'object_type' and 'object_id' fields");
            return;
        }

        if (!Enum.TryParse<SyncObjectType>(objectTypeStr, true, out var objectType))
        {
            await SendErrorAsync(webSocket, $"Invalid object type: {objectTypeStr}");
            return;
        }

        var success = _subscriptionManager.AddSubscription(connectionId, objectType, objectId);
        if (success)
        {
            await SendEventAsync(webSocket, "subscribed", objectTypeStr, objectId);
        }
        else
        {
            await SendErrorAsync(webSocket, "Failed to subscribe (limit reached?)");
        }
    }

    private async Task HandleUnsubscribeAsync(WebSocket webSocket, string connectionId, InboundMessage message)
    {
        var objectTypeStr = message.ObjectType;
        var objectId = message.ObjectId;

        if (string.IsNullOrEmpty(objectTypeStr) || string.IsNullOrEmpty(objectId))
        {
            await SendErrorAsync(webSocket, "Unsubscribe requires 'object_type' and 'object_id' fields");
            return;
        }

        if (!Enum.TryParse<SyncObjectType>(objectTypeStr, true, out var objectType))
        {
            await SendErrorAsync(webSocket, $"Invalid object type: {objectTypeStr}");
            return;
        }

        _subscriptionManager.RemoveSubscription(connectionId, objectType, objectId);
        await SendEventAsync(webSocket, "unsubscribed", objectTypeStr, objectId);
    }

    private async Task SendEventAsync(WebSocket webSocket, string eventType, string objectType, string objectId)
    {
        var json = _messageSerializer.SerializeEvent(eventType, objectType, objectId);
        var bytes = Encoding.UTF8.GetBytes(json);
        await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task SendErrorAsync(WebSocket webSocket, string error)
    {
        var json = _messageSerializer.SerializeEvent("error", "system", "unknown", new { message = error });
        var bytes = Encoding.UTF8.GetBytes(json);
        await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
