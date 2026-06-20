using OpenSync.Sdk;
using OpenSync.Sdk.Auth;
using OpenSync.Sdk.Transports;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("==============================================");
Console.WriteLine("  OpenSync Demo - Real-time Sync Showcase");
Console.WriteLine("  Like Twilio Sync for .NET");
Console.WriteLine("==============================================");
Console.ResetColor();
Console.WriteLine();

var baseUrl = "http://localhost:5000";
var tokenProvider = new TokenProvider("demo-key");
using var client = new OpenSyncClient(baseUrl, tokenProvider, TransportType.WebSocket);

client.OnConnected += (_, _) => WriteColored("Connected to server!", ConsoleColor.Green);
client.OnDisconnected += (_, _) => WriteColored("Disconnected from server", ConsoleColor.Yellow);

await client.ConnectAsync();
await Task.Delay(500);

Console.WriteLine();

await DemonstrateDocument(client);

await DemonstrateStream(client);

await DemonstrateChannel(client);

Console.WriteLine();
WriteColored("Demo completed successfully!", ConsoleColor.Green);
WriteColored("Press any key to exit...", ConsoleColor.DarkGray);
Console.ReadKey();

static async Task DemonstrateDocument(OpenSyncClient client)
{
    WriteHeader("1. SyncDocument - Shared Configuration Object");

    var doc = await client.GetDocumentAsync("app-config");
    doc.OnUpdated += (_, e) => WriteColored($"  [Event] Document updated: {e.Data}", ConsoleColor.Magenta);
    doc.OnRemoved += (_, _) => WriteColored("  [Event] Document removed", ConsoleColor.Red);

    await doc.SubscribeAsync();
    await Task.Delay(200);

    WriteColored("  Setting document data...", ConsoleColor.Gray);
    await doc.SetAsync(new
    {
        theme = "dark",
        language = "en",
        features = new[] { "chat", "sync", "presence" }
    });

    await Task.Delay(500);
    WriteColored("  Document synced! Changes are pushed in real-time to all subscribers.", ConsoleColor.DarkGray);
    Console.WriteLine();
}

static async Task DemonstrateStream(OpenSyncClient client)
{
    WriteHeader("2. SyncStream - Real-time Event Stream");

    var stream = await client.GetStreamAsync("notifications");
    stream.OnMessage += (_, e) => WriteColored($"  [Event] Stream message: {e.Data}", ConsoleColor.Cyan);

    await stream.SubscribeAsync();
    await Task.Delay(200);

    WriteColored("  Publishing stream messages...", ConsoleColor.Gray);
    await stream.PublishAsync(new { type = "info", message = "User joined the workspace", timestamp = DateTime.UtcNow });
    await Task.Delay(300);
    await stream.PublishAsync(new { type = "alert", message = "Task completed", timestamp = DateTime.UtcNow });
    await Task.Delay(300);
    await stream.PublishAsync(new { type = "success", message = "File uploaded", timestamp = DateTime.UtcNow });

    await Task.Delay(500);
    WriteColored("  Stream messages delivered in real-time to all subscribers.", ConsoleColor.DarkGray);
    Console.WriteLine();
}

static async Task DemonstrateChannel(OpenSyncClient client)
{
    WriteHeader("3. SyncChannel - Real-time Chat with Presence");

    var channel = await client.GetChannelAsync("general");
    channel.OnMessage += (_, e) => WriteColored($"  [Event] Channel message: {e.Data}", ConsoleColor.Yellow);
    channel.OnMemberJoined += (_, e) => WriteColored($"  [Event] Member joined: {e.Data}", ConsoleColor.Green);
    channel.OnMemberLeft += (_, e) => WriteColored($"  [Event] Member left: {e.Data}", ConsoleColor.Red);

    await channel.SubscribeAsync();
    await Task.Delay(200);

    WriteColored("  Joining channel 'general'...", ConsoleColor.Gray);
    await channel.JoinAsync(new { displayName = "DemoUser", avatar = "https://example.com/avatar.png" });

    await Task.Delay(300);

    WriteColored("  Sending messages...", ConsoleColor.Gray);
    await channel.SendMessageAsync(new
    {
        author = "DemoUser",
        text = "Hello everyone! This is a real-time message via OpenSync!",
        timestamp = DateTime.UtcNow
    });

    await Task.Delay(200);
    await channel.SendMessageAsync(new
    {
        author = "DemoUser",
        text = "OpenSync supports Document, List, Map, Stream, and Channel primitives.",
        timestamp = DateTime.UtcNow
    });

    await Task.Delay(500);
    WriteColored("  Messages broadcast to all channel members in real-time.", ConsoleColor.DarkGray);

    await Task.Delay(200);
    WriteColored("  Leaving channel...", ConsoleColor.Gray);
    await channel.LeaveAsync();
    WriteColored("  Left channel. Presence updated for remaining members.", ConsoleColor.DarkGray);
    Console.WriteLine();
}

static void WriteHeader(string text)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(text);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(new string('-', text.Length));
    Console.ResetColor();
}

static void WriteColored(string text, ConsoleColor color)
{
    var original = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ForegroundColor = original;
}
