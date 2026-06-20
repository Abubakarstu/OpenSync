using OpenSync.Sdk.Events;

namespace OpenSync.Sdk;

public class SyncMap
{
    private readonly OpenSyncClient _client;
    private readonly string _uniqueName;

    public event EventHandler<ItemChangedEventArgs>? OnItemAdded;
    public event EventHandler<ItemChangedEventArgs>? OnItemUpdated;
    public event EventHandler<ItemChangedEventArgs>? OnItemRemoved;

    public SyncMap(OpenSyncClient client, string uniqueName)
    {
        _client = client;
        _uniqueName = uniqueName;
        _client.OnEvent += HandleEvent;
    }

    private void HandleEvent(object? sender, SyncEventArgs args)
    {
        if (args.ObjectType == "map" && args.ObjectId == _uniqueName)
        {
            switch (args.Event)
            {
                case "item_added": OnItemAdded?.Invoke(this, new ItemChangedEventArgs(args.Data)); break;
                case "item_updated": OnItemUpdated?.Invoke(this, new ItemChangedEventArgs(args.Data)); break;
                case "item_removed": OnItemRemoved?.Invoke(this, new ItemChangedEventArgs(args.Data)); break;
            }
        }
    }

    public async Task SubscribeAsync() => await _client.SubscribeAsync("map", _uniqueName);
}
