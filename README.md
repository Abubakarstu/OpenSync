# OpenSync — Real-Time Sync Library for .NET

[![CI](https://github.com/Abubakarstu/OpenSync/actions/workflows/ci.yml/badge.svg)](https://github.com/Abubakarstu/OpenSync/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/OpenSync.AspNetCore)](https://www.nuget.org/packages/OpenSync.AspNetCore)
[![License](https://img.shields.io/github/license/Abubakarstu/OpenSync)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download)

OpenSync is a **general-purpose real-time data synchronization library** for .NET 8 built with Clean Architecture. It provides 5 primitives — Documents, Lists, Maps, Streams, and Channels — that cover every real-time use case from chat and collaborative editing to live dashboards and IoT.

Run it as a **standalone microservice** or **embed it in any ASP.NET Core app** in 3 lines of code.

---

## Features

- **5 Primitives** — Documents (JSON objects), Lists (ordered items), Maps (key-value), Streams (ephemeral pub/sub), Channels (presence + messaging)
- **3 Transports** — WebSocket, Server-Sent Events, HTTP Long-Polling (client chooses)
- **Clean Architecture** — Core → Application → Infrastructure → API, fully testable
- **Optimistic Concurrency** — Revision-based conflict detection
- **Real-Time Events** — Change notifications pushed to subscribed clients
- **Channel Presence** — Join/leave, metadata, member tracking
- **TTL Expiry** — Auto-expire any object with configurable cleanup
- **Heartbeat** — Bidirectional ping/pong every 30 seconds
- **Authentication** — JWT tokens with scoped permissions + API keys
- **Rate Limiting** — Sliding window per client
- **Multi-Instance Scaling** — In-process (default) or Redis backplane
- **Pluggable Storage** — PostgreSQL (primary), SQLite (dev), SQL Server (alt)
- **NuGet Packages** — Embed server (`OpenSync.AspNetCore`) or connect client (`OpenSync.Sdk`)
- **JavaScript SDK** — TypeScript client for browser/Node.js

---

## Quick Start

### Run as a standalone server

```bash
git clone https://github.com/Abubakarstu/OpenSync.git
cd OpenSync/src/OpenSync.Api
dotnet run
```

Swagger UI: `https://localhost:5001/swagger`

### Embed in your ASP.NET Core app

```bash
dotnet add package OpenSync.AspNetCore
```

```csharp
// Program.cs
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

var channel = await client.Channels.GetAsync("chat-room");
await channel.JoinAsync(new { name = "Alice" });
await channel.SendMessageAsync(new { text = "Hello!" });
```

### JavaScript client

```ts
const client = new OpenSyncClient("https://localhost:5001");
await client.connect(token);

const doc = client.document("settings");
doc.onUpdated = (data) => console.log("Updated:", data);
await doc.set({ theme: "dark" });
```

---

## Primitives

| Primitive | Description | Use Cases |
|-----------|-------------|-----------|
| **Document** | Single JSON object with revision control | User profiles, settings, game state |
| **List** | Ordered collection of JSON items | Chat messages, activity feeds, todo lists |
| **Map** | Unordered key-value dictionary | Presence, lookup tables, caches |
| **Stream** | Ephemeral fire-and-forget pub/sub | Notifications, cursors, telemetry |
| **Channel** | Named room with presence + messaging | Chat rooms, multiplayer lobbies |

---

## Architecture

```
┌─────────────────────────────────────────────────┐
│                   API (ASP.NET Core)             │
│  Controllers · Middleware · Swagger · WebSocket  │
├─────────────────────────────────────────────────┤
│              Infrastructure                      │
│  EF Core · Transports · Backplane · Auth · TTL   │
├─────────────────────────────────────────────────┤
│              Application (CQRS)                  │
│  MediatR · FluentValidation · Pipeline Behaviors │
├─────────────────────────────────────────────────┤
│              Core (Domain)                       │
│  Entities · Value Objects · Events · Interfaces  │
└─────────────────────────────────────────────────┘
```

---

## REST API

All endpoints are under `/api/v1/sync/services/{serviceId}`:

```
POST   /documents          Create a document
GET    /documents/{id}     Get a document
PATCH  /documents/{id}     Partial update (merge)
PUT    /documents/{id}     Full replace
DELETE /documents/{id}     Delete

POST   /lists              Create a list
POST   /lists/{id}/items   Append item
GET    /lists/{id}/items   List items (paginated)
DELETE /lists/{id}/items/{itemId}  Remove item

POST   /maps               Create a map
PUT    /maps/{id}/items/{key}  Set item (upsert)
DELETE /maps/{id}/items/{key}  Remove item

POST   /streams            Create a stream
POST   /streams/{id}/messages   Publish message

POST   /channels           Create a channel
POST   /channels/{id}/members   Join
PUT    /channels/{id}/members/{identity}  Update presence
POST   /channels/{id}/messages  Broadcast

POST   /tokens             Generate JWT token
```

---

## Configuration

```json
{
  "OpenSync": {
    "Database": {
      "Provider": "PostgreSQL",
      "ConnectionString": "Host=localhost;Database=opensync"
    },
    "Limits": {
      "MaxDocumentSizeBytes": 16384
    },
    "Heartbeat": {
      "IntervalSeconds": 30,
      "TimeoutSeconds": 10
    },
    "Backplane": {
      "Type": "InProcess"
    }
  }
}
```

---

## Documentation

| Category | Link |
|----------|------|
| **User Manual** | [docs/UserManual/](docs/UserManual/README.md) — quick start, config, API, SDK, examples |
| **Development** | [docs/Development/](docs/Development/README.md) — architecture, deployment, contributing |

## NuGet Packages

| Package | Description |
|---------|-------------|
| `OpenSync.AspNetCore` | Embed OpenSync server in any ASP.NET Core app |

---

## Building from Source

```bash
dotnet build OpenSync.sln
dotnet test OpenSync.sln
```

---

## License

MIT — see [LICENSE](LICENSE)

---

## Author

**Muhammad Abubakar**  
[LinkedIn](https://www.linkedin.com/in/muhammadabubakar-dotnetdeveloper/)
