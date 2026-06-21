# OpenSync User Manual

## Author

**Muhammad Abubakar** — [LinkedIn](https://www.linkedin.com/in/muhammadabubakar-dotnetdeveloper/)

---

OpenSync is a real-time data synchronization library for .NET 8. This manual covers everything you need to use OpenSync in your applications.

## Sections

| Section | Description |
|---------|-------------|
| [Getting Started](GettingStarted.md) | Install, configure, and run OpenSync |
| [API Reference](ApiReference.md) | REST endpoints for all primitives |
| [Client SDK](ClientSdk.md) | .NET and JavaScript client libraries |
| [Examples](Examples/) | Chat app, live dashboard, and more |

## Quick Overview

### Embed in your ASP.NET Core app

```bash
dotnet add package OpenSync.AspNetCore
```

```csharp
builder.Services.AddOpenSync(configuration);
app.UseOpenSync();
```

### Connect a .NET client

```bash
dotnet add package OpenSync.Sdk
```

```csharp
var client = new OpenSyncClient("https://localhost:5001", tokenProvider);
var doc = await client.Documents.GetAsync("settings");
await doc.SetAsync(new { theme = "dark", language = "en" });
```

### JavaScript client

```ts
const client = new OpenSyncClient("https://localhost:5001");
await client.connect(token);
const doc = client.document("settings");
doc.onUpdated = (data) => console.log("Updated:", data);
await doc.set({ theme: "dark" });
```

## Primitives

| Primitive | Description | Use Cases |
|-----------|-------------|-----------|
| **Document** | Single JSON object with revision control | User profiles, settings, game state |
| **List** | Ordered collection of JSON items | Chat messages, activity feeds, todo lists |
| **Map** | Unordered key-value dictionary | Presence, lookup tables, caches |
| **Stream** | Ephemeral fire-and-forget pub/sub | Notifications, cursors, telemetry |
| **Channel** | Named room with presence + messaging | Chat rooms, multiplayer lobbies |

## Transports

Clients can connect via WebSocket, Server-Sent Events (SSE), or HTTP Long-Polling.
