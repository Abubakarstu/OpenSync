# OpenSync Demo

This demo showcases OpenSync real-time synchronization with a server and client.

## Server

The server demonstrates:
- Creating and managing sync services
- Handling WebSocket connections for real-time updates
- Document operations (create, update, subscribe)
- Stream operations (publish, subscribe)
- Channel operations (join, leave, message broadcast)

To run the server:
```bash
dotnet run --project src/OpenSync.Demo.Server
```

## Client

The client demonstrates:
- Connecting to the server
- Generating authentication tokens
- Subscribing to sync events
- Performing all 5 primitive operations

To run the client:
```bash
dotnet run --project src/OpenSync.Demo.Client
```

## Prerequisites

- .NET 8 SDK
- Open the solution in Visual Studio or run commands from the project root

## How It Works

The demo uses OpenSync's 5 primitives:
1. **SyncDocument** - Shared config object with real-time updates
2. **SyncStream** - Publish/subscribe event stream
3. **SyncChannel** - Chat with join/leave presence + messaging
4. **SyncList** - Ordered list of items
5. **SyncMap** - Key-value store

The server implements all the API endpoints, and the client connects to demonstrate real-time synchronization.
