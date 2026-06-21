# OpenSync.AspNetCore

Real-time data synchronization infrastructure for .NET 8 ASP.NET Core applications.

## Author

**Muhammad Abubakar**  
Senior .NET Developer  
[LinkedIn](https://www.linkedin.com/in/muhammadabubakar-dotnetdeveloper/)

---

## Features

- **5 sync primitives** — Document, Counter, Map, List, Set
- **3 transports** — WebSocket, Server-Sent Events (SSE), HTTP Long-Polling
- **Clean Architecture** — Core, Application, Infrastructure, API layers
- **PostgreSQL** primary database with Entity Framework Core
- **Optimistic concurrency** via revision tracking
- **TTL expiry** — automatic cleanup of stale data
- **Heartbeat** — connection health monitoring
- **Single package** — all dependencies bundled, zero external NuGet references

## Full Documentation

- [User Manual](https://github.com/Abubakarstu/OpenSync/blob/main/docs/UserManual/README.md) — getting started, configuration, API reference, client SDK, examples
- [Development Guide](https://github.com/Abubakarstu/OpenSync/blob/main/docs/Development/README.md) — architecture, deployment, contributing

## Quick Start

### 1. Install

```bash
dotnet add package OpenSync.AspNetCore
```

### 2. Register in `Program.cs`

```csharp
using OpenSync.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenSync(builder.Configuration);

var app = builder.Build();

app.UseWebSockets();
app.UseOpenSync();

app.Run();
```

### 3. Configure `appsettings.json`

```json
{
  "OpenSync": {
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Database=opensync;Username=postgres;Password=***"
    },
    "SyncOptions": {
      "DefaultTtlMinutes": 60,
      "HeartbeatIntervalSeconds": 30,
      "MaxConnectionsPerUser": 10
    }
  }
}
```

## Package Contents

The `OpenSync.AspNetCore` package bundles:

- `OpenSync.Core` — domain models, interfaces, primitives
- `OpenSync.Application` — CQRS commands/queries, MediatR handlers, validation
- `OpenSync.Infrastructure` — EF Core, PostgreSQL, Redis, JWT auth, Serilog
- `OpenSync.Api` — ASP.NET Core controllers, WebSocket middleware, SSE

All transitive NuGet dependencies (EF Core, MediatR, Npgsql, Mapster, FluentValidation, Serilog, Swashbuckle, etc.) are included.

## License

MIT
