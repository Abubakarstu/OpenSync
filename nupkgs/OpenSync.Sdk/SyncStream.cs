using OpenSync.Sdk.Events;

namespace OpenSync.Sdk;

public class SyncStream
{
    private readonly OpenSyncClient _client;
    private readonly string _uniqueName;

    public event EventHandler<MessageEventArgs>? OnMessage;

    public SyncStream(OpenSyncClient client, string uniqueName)
    {
        _client = client;
        _uniqueName = uniqueName;
        _client.OnEvent += HandleEvent;
    }

    private void HandleEvent(object? sender, SyncEventArgs args)
    {
        if (args.Event == "stream_message" && args.ObjectId == _uniqueName)
            OnMessage?.Invoke(this, new MessageEventArgs(args.Data));
    }

    public async Task PublishAsync(object data)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data);
        await _client.HttpPostAsync<object>($"/streams/{_uniqueName}/messages", new { data = json });
    }

    public async Task SubscribeAsync() => await _client.SubscribeAsync("stream", _uniqueName);
}
