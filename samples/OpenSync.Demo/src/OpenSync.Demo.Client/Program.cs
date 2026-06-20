using System.Text.Json;
using OpenSync.Sdk;
using OpenSync.Sdk.Events;

var client = new OpenSyncClient("http://localhost:5000", TransportType.WebSocket);

Console.WriteLine("OpenSync Demo Client");
Console.WriteLine("=====================");

// Generate a token
var token = await client.GenerateTokenAsync("demo-user", "demo", new Dictionary<string, string[]>(), 60);
Console.WriteLine($"Generated token: {token[..20]}...")

// Subscribe to document changes
await client.SubscribeAsync<SyncDocumentEvent>("$documents.test-doc", async (e) =>
{
    Console.WriteLine($"Document updated: {e.Data}");
});

// Create a document
var docId = await client.CreateDocumentAsync("test-doc", "{\"message\": \"Hello from client!\"}");
Console.WriteLine($"Created document with ID: {docId}")

// Subscribe to stream messages
await client.SubscribeAsync<StreamMessageEvent>("$streams.chat", async (e) =>
{
    Console.WriteLine($"Stream message: {e.Data}");
});

// Publish to stream
await client.PublishMessageAsync("chat", "{\"user\": \"client\", \"text\": \"Hi there!\"}");
Console.WriteLine("Published message to stream")

// Subscribe to channel
await client.SubscribeAsync<ChannelMemberJoinedEvent>("$channels/general", async (e) =>
{
    Console.WriteLine($"User {e.Identity} joined the channel");
});

await client.JoinChannelAsync("general", "client", "{\"role\": \"user\"}");
Console.WriteLine("Joined channel")

// Send message to channel
await client.BroadcastMessageAsync("general", "{\"user\": \"client\", \"text\": \"Hello everyone!\"}");
Console.WriteLine("Sent message to channel")

await client.LeaveChannelAsync("general");
Console.WriteLine("Left channel")

Console.WriteLine("\nDemo completed successfully!")
