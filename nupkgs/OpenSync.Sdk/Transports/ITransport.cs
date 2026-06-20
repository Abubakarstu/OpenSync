namespace OpenSync.Sdk.Transports;

public enum TransportType { WebSocket, Sse, LongPolling }

public interface ITransport
{
    event EventHandler<string>? OnMessage;
    event EventHandler? OnConnected;
    event EventHandler? OnDisconnected;
    Task ConnectAsync(string token, CancellationToken cancellationToken = default);
    Task DisconnectAsync();
    Task SendAsync(string message, CancellationToken cancellationToken = default);
}
