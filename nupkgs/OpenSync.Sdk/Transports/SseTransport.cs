namespace OpenSync.Sdk.Transports;

public class SseTransport : ITransport
{
    private readonly string _baseUrl;
    private HttpClient? _httpClient;
    private CancellationTokenSource? _cts;

    public event EventHandler<string>? OnMessage;
    public event EventHandler? OnConnected;
    public event EventHandler? OnDisconnected;

    public SseTransport(string baseUrl) => _baseUrl = baseUrl;

    public Task ConnectAsync(string token, CancellationToken cancellationToken = default)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        OnConnected?.Invoke(this, EventArgs.Empty);
        _ = Task.Run(() => ReceiveLoop(_cts.Token), cancellationToken);

        return Task.CompletedTask;
    }

    private async Task ReceiveLoop(CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/v1/sync/events");
            using var response = await _httpClient!.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            while (!cancellationToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(cancellationToken);
                if (line == null) break;
                if (line.StartsWith("data: "))
                {
                    var data = line[6..];
                    OnMessage?.Invoke(this, data);
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception)
        {
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }

    public Task DisconnectAsync()
    {
        _cts?.Cancel();
        _httpClient?.Dispose();
        OnDisconnected?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }

    public Task SendAsync(string message, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
