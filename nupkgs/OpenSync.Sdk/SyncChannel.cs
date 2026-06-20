using OpenSync.Sdk.Events;

namespace OpenSync.Sdk;

public class SyncChannel
{
    private readonly OpenSyncClient _client;
    private readonly string _uniqueName;

    public event EventHandler<PresenceChangedEventArgs>? OnMemberJoined;
    public event EventHandler<PresenceChangedEventArgs>? OnMemberLeft;
    public event EventHandler<PresenceChangedEventArgs>? OnPresenceUpdated;
    public event EventHandler<MessageEventArgs>? OnMessage;

    public SyncChannel(OpenSyncClient client, string uniqueName)
    {
        _client = client;
        _uniqueName = uniqueName;
        _client.OnEvent += HandleEvent;
    }

    private void HandleEvent(object? sender, SyncEventArgs args)
    {
        if (args.ObjectType != "channel" || args.ObjectId != _uniqueName) return;

        switch (args.Event)
        {
            case "member_joined": OnMemberJoined?.Invoke(this, new PresenceChangedEventArgs(args.Data)); break;
            case "member_left": OnMemberLeft?.Invoke(this, new PresenceChangedEventArgs(args.Data)); break;
            case "presence_updated": OnPresenceUpdated?.Invoke(this, new PresenceChangedEventArgs(args.Data)); break;
            case "message_broadcast": OnMessage?.Invoke(this, new MessageEventArgs(args.Data)); break;
        }
    }

    public async Task JoinAsync(object? metadata = null)
    {
        var json = metadata != null ? System.Text.Json.JsonSerializer.Serialize(metadata) : null;
        await _client.HttpPostAsync<object>($"/channels/{_uniqueName}/members", new { identity = "default", metadata = json });
    }

    public async Task LeaveAsync()
        => await _client.HttpPostAsync<object>($"/channels/{_uniqueName}/leave", new { });

    public async Task SendMessageAsync(object data)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data);
        await _client.HttpPostAsync<object>($"/channels/{_uniqueName}/messages", new { data = json });
    }

    public async Task SubscribeAsync() => await _client.SubscribeAsync("channel", _uniqueName);
}
