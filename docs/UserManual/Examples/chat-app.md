# Chat App with OpenSync

```csharp
// Server: embed OpenSync
builder.Services.AddOpenSync(configuration);
app.UseOpenSync();
```

```csharp
// Client: .NET SDK
var client = new OpenSyncClient("https://localhost:5001", tokenProvider);

var channel = await client.Channels.GetOrCreateAsync("lobby");

await channel.JoinAsync(new { name = "Alice", avatar = "alice.png" });

channel.OnMessage += (_, args) =>
    Console.WriteLine($"[{args.Data["sender"]}] {args.Data["text"]}");

channel.OnMemberJoined += (_, args) =>
    Console.WriteLine($"{args.Metadata["name"]} joined");

channel.OnMemberLeft += (_, args) =>
    Console.WriteLine($"{args.Identity} left");

// Send message
await channel.SendMessageAsync(new
{
    sender = "Alice",
    text = "Hello everyone!"
});
```
