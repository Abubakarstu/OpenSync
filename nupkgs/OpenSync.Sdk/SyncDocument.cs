using OpenSync.Sdk.Events;

namespace OpenSync.Sdk;

public class SyncDocument
{
    private readonly OpenSyncClient _client;
    private readonly string _uniqueName;

    public event EventHandler<ItemChangedEventArgs>? OnUpdated;
    public event EventHandler<ItemChangedEventArgs>? OnRemoved;

    public SyncDocument(OpenSyncClient client, string uniqueName)
    {
        _client = client;
        _uniqueName = uniqueName;
        _client.OnEvent += HandleEvent;
    }

    private void HandleEvent(object? sender, SyncEventArgs args)
    {
        if (args.ObjectType == "document" && args.ObjectId == _uniqueName)
        {
            if (args.Event == "object_updated")
                OnUpdated?.Invoke(this, new ItemChangedEventArgs(args.Data));
            else if (args.Event == "object_removed")
                OnRemoved?.Invoke(this, new ItemChangedEventArgs(args.Data));
        }
    }

    public async Task SetAsync(object data, long? expectedRevision = null)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data);
        await _client.HttpPostAsync<object>($"/documents/{_uniqueName}", new { data = json, expected_revision = expectedRevision });
    }

    public async Task SubscribeAsync() => await _client.SubscribeAsync("document", _uniqueName);
}
