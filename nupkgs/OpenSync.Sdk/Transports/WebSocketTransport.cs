using System.Net.WebSockets;
using System.Text;

namespace OpenSync.Sdk.Transports;

public class WebSocketTransport : ITransport
{
    private ClientWebSocket? _ws;
    private readonly string _url;
    private CancellationTokenSource? _cts;

    public event EventHandler<string>? OnMessage;
    public event EventHandler? OnConnected;
    public event EventHandler? OnDisconnected;

    public WebSocketTransport(string baseUrl)
    {
        var wsUrl = baseUrl.Replace("https://", "wss://").Replace("http://", "ws://");
        _url = $"{wsUrl}/ws";
    }

    public async Task ConnectAsync(string token, CancellationToken cancellationToken = default)
    {
        _ws = new ClientWebSocket();
        _ws.Options.SetRequestHeader("Authorization", $"Bearer {token}");
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        await _ws.ConnectAsync(new Uri(_url), _cts.Token);
        OnConnected?.Invoke(this, EventArgs.Empty);

        _ = Task.Run(() => ReceiveLoop(_cts.Token));
    }

    private async Task ReceiveLoop(CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 16];
        try
        {
            while (_ws?.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    OnDisconnected?.Invoke(this, EventArgs.Empty);
                    break;
                }

                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessage?.Invoke(this, msg);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }

    public async Task DisconnectAsync()
    {
        _cts?.Cancel();
        if (_ws?.State == WebSocketState.Open)
        {
            await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", CancellationToken.None);
        }
        _ws?.Dispose();
        _ws = null;
        OnDisconnected?.Invoke(this, EventArgs.Empty);
    }

    public async Task SendAsync(string message, CancellationToken cancellationToken = default)
    {
        if (_ws?.State == WebSocketState.Open)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
