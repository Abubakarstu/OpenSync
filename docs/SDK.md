# OpenSync SDK

## .NET Client SDK

Install: `dotnet add package OpenSync.Sdk`

```csharp
using OpenSync.Sdk;

// Create client with token provider
var tokenProvider = new TokenProvider(async () =>
{
    // Fetch a token from your auth endpoint
    var httpClient = new HttpClient();
    var response = await httpClient.PostAsJsonAsync("https://your-server/api/v1/sync/tokens",
        new { identity = "user-123" });
    var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
    return result!.Data.Token;
});

var client = new OpenSyncClient("https://your-server", tokenProvider);

// Document
var doc = await client.Documents.GetOrCreateAsync("user-settings", new { theme = "dark" });
doc.OnUpdated += (_, args) => Console.WriteLine($"Updated: {args.Data}");
await doc.SetAsync(new { theme = "light", language = "en" });

// List
var list = await client.Lists.GetOrCreateAsync("notifications");
await list.AppendAsync(new { text = "New message", timestamp = DateTime.UtcNow });

// Map
var map = await client.Maps.GetOrCreateAsync("presence");
await map.SetItemAsync("user-123", new { name = "Alice", status = "online" });

// Stream
var stream = await client.Streams.GetOrCreateAsync("cursor-positions");
stream.OnMessage += (_, args) => Console.WriteLine($"Stream: {args.Data}");
await stream.PublishAsync(new { x = 100, y = 200 });

// Channel
var channel = await client.Channels.GetOrCreateAsync("general");
await channel.JoinAsync(new { name = "Alice" });
channel.OnMessage += (_, args) => Console.WriteLine($"Chat: {args.Data}");
await channel.SendMessageAsync(new { text = "Hello everyone!" });
```

## JavaScript/TypeScript SDK

Located in `clients/OpenSync.Client.Js/`.

```ts
import { OpenSyncClient } from './OpenSyncClient';

const client = new OpenSyncClient('https://your-server', {
    getToken: async () => {
        const res = await fetch('https://your-server/api/v1/sync/tokens', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ identity: 'user-123' })
        });
        const data = await res.json();
        return data.data.token;
    }
});

await client.connect();

// Document
const doc = client.document('settings');
doc.onUpdated = (data) => console.log('Doc updated:', data);
await doc.set({ theme: 'dark' });

// Channel
const channel = client.channel('general');
await channel.join({ name: 'Alice' });
channel.onMessage = (data) => console.log('Message:', data);
await channel.sendMessage({ text: 'Hello!' });
```
