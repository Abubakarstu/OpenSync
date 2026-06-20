using OpenSync.Sdk.Auth;
using OpenSync.Sdk.Events;
using OpenSync.Sdk.Serialization;
using OpenSync.Sdk.Transports;

namespace OpenSync.Sdk;

public class OpenSyncClient : IDisposable
{
    private readonly string _baseUrl;
    private readonly ITokenProvider _tokenProvider;
    private readonly ITransport _transport;
    private readonly JsonEventSerializer _serializer;
    private readonly HttpClient _httpClient;
    private string? _connectionId;

    public event EventHandler<SyncEventArgs>? OnEvent;
    public event EventHandler<EventArgs>? OnConnected;
    public event EventHandler<EventArgs>? OnDisconnected;

    public OpenSyncClient(string baseUrl, ITokenProvider tokenProvider, TransportType transportType = TransportType.WebSocket)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _tokenProvider = tokenProvider;
        _serializer = new JsonEventSerializer();
        _httpClient = new HttpClient();
        _transport = TransportFactory.Create(transportType, _baseUrl, _serializer, tokenProvider);
        _transport.OnMessage += HandleMessage;
        _transport.OnConnected += (s, e) => OnConnected?.Invoke(this, e);
        _transport.OnDisconnected += (s, e) => OnDisconnected?.Invoke(this, e);
    }

    private void HandleMessage(object? sender, string json)
    {
        var evt = _serializer.DeserializeEvent(json);
        if (evt != null)
            OnEvent?.Invoke(this, evt);
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var token = await _tokenProvider.GetTokenAsync();
        await _transport.ConnectAsync(token, cancellationToken);
        _connectionId = Guid.NewGuid().ToString("N");
    }

    public async Task DisconnectAsync()
    {
        await _transport.DisconnectAsync();
        _connectionId = null;
    }

    public async Task SubscribeAsync(string objectType, string objectId, CancellationToken cancellationToken = default)
    {
        var msg = _serializer.Serialize(new { action = "subscribe", object_type = objectType, object_id = objectId });
        await _transport.SendAsync(msg, cancellationToken);
    }

    public async Task UnsubscribeAsync(string objectType, string objectId, CancellationToken cancellationToken = default)
    {
        var msg = _serializer.Serialize(new { action = "unsubscribe", object_type = objectType, object_id = objectId });
        await _transport.SendAsync(msg, cancellationToken);
    }

    public Task<SyncDocument> GetDocumentAsync(string uniqueName)
        => Task.FromResult(new SyncDocument(this, uniqueName));

    public Task<SyncList> GetListAsync(string uniqueName)
        => Task.FromResult(new SyncList(this, uniqueName));

    public Task<SyncMap> GetMapAsync(string uniqueName)
        => Task.FromResult(new SyncMap(this, uniqueName));

    public Task<SyncStream> GetStreamAsync(string uniqueName)
        => Task.FromResult(new SyncStream(this, uniqueName));

    public Task<SyncChannel> GetChannelAsync(string uniqueName)
        => Task.FromResult(new SyncChannel(this, uniqueName));

    public async Task<T> HttpGetAsync<T>(string path)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/sync{path}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return _serializer.Deserialize<T>(json);
    }

    public async Task<T> HttpPostAsync<T>(string path, object body)
    {
        var content = new StringContent(_serializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/sync{path}", content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return _serializer.Deserialize<T>(json);
    }

    internal async Task SendAsync(string message, CancellationToken cancellationToken = default)
        => await _transport.SendAsync(message, cancellationToken);

    public void Dispose()
    {
        _transport?.DisconnectAsync().GetAwaiter().GetResult();
        _httpClient?.Dispose();
    }
}
