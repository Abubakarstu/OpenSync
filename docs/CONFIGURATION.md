# OpenSync Configuration

## appsettings.json

```json
{
  "OpenSync": {
    "Database": {
      "Provider": "PostgreSQL",
      "ConnectionString": "Host=localhost;Database=opensync;Username=postgres;Password=postgres"
    },
    "Limits": {
      "MaxDocumentSizeBytes": 16384,
      "MaxListItemDataSizeBytes": 16384,
      "MaxMapItemDataSizeBytes": 16384,
      "MaxStreamMessageSizeBytes": 4096,
      "MaxListItems": 1000,
      "MaxMapItems": 10000,
      "MaxSubscriptionsPerConnection": 200,
      "MaxConnectionsPerService": 10000,
      "DefaultRateLimitPerSecond": 100
    },
    "Heartbeat": {
      "IntervalSeconds": 30,
      "TimeoutSeconds": 10
    },
    "Ttl": {
      "CleanupIntervalSeconds": 60
    },
    "Backplane": {
      "Type": "InProcess",
      "Redis": {
        "ConnectionString": "localhost:6379",
        "ChannelPrefix": "opensync:"
      }
    }
  }
}
```

## Database Providers

| Provider | Value | Notes |
|----------|-------|-------|
| PostgreSQL | `PostgreSQL` | Default, production |
| SQLite | `Sqlite` | Development only |
| SQL Server | `SqlServer` | Alternative |

## Embedding Configuration

When embedding via `OpenSync.AspNetCore`:

```csharp
// From appsettings.json
builder.Services.AddOpenSync(configuration);

// Or inline
builder.Services.AddOpenSync(options =>
{
    options.Database.Provider = "PostgreSQL";
    options.Database.ConnectionString = "Host=localhost;Database=opensync";
    options.Limits.MaxDocumentSizeBytes = 32768;
    options.Heartbeat.IntervalSeconds = 15;
});
```

## Environment Variables

| Variable | Overrides |
|----------|-----------|
| `ConnectionStrings__OpenSync` | Database connection string |
| `OpenSync__Database__Provider` | Database provider |
| `OpenSync__Backplane__Type` | Backplane type |
| `OpenSync__Limits__MaxDocumentSizeBytes` | Max document size |
