namespace OpenSync.Infrastructure.Configuration;

public class OpenSyncOptions
{
    public HeartbeatOptions Heartbeat { get; set; } = new();
    public TtlOptions Ttl { get; set; } = new();
    public LimitsOptions Limits { get; set; } = new();
    public BackplaneOptions Backplane { get; set; } = new();
    public DatabaseOptions Database { get; set; } = new();
}

public class HeartbeatOptions
{
    public int IntervalSeconds { get; set; } = 30;
    public int TimeoutSeconds { get; set; } = 10;
}

public class TtlOptions
{
    public int CleanupIntervalSeconds { get; set; } = 60;
}

public class LimitsOptions
{
    public int MaxDocumentSizeBytes { get; set; } = 16384;
    public int MaxListItemDataSizeBytes { get; set; } = 16384;
    public int MaxMapItemDataSizeBytes { get; set; } = 16384;
    public int MaxStreamMessageSizeBytes { get; set; } = 4096;
    public int MaxListItems { get; set; } = 1000;
    public int MaxMapItems { get; set; } = 10000;
    public int MaxSubscriptionsPerConnection { get; set; } = 200;
    public int MaxConnectionsPerService { get; set; } = 10000;
    public int DefaultRateLimitPerSecond { get; set; } = 100;
}

public class BackplaneOptions
{
    public string Type { get; set; } = "InProcess";
    public RedisOptions? Redis { get; set; }
}

public class RedisOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";
    public string ChannelPrefix { get; set; } = "opensync:";
}

public class DatabaseOptions
{
    public string Provider { get; set; } = "PostgreSQL";
    public string ConnectionString { get; set; } = string.Empty;
}
