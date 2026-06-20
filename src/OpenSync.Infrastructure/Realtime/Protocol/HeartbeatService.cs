using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenSync.Infrastructure.Configuration;
using OpenSync.Infrastructure.Realtime.Connection;

namespace OpenSync.Infrastructure.Realtime.Protocol;

public class HeartbeatService : BackgroundService
{
    private readonly IConnectionManager _connectionManager;
    private readonly ILogger<HeartbeatService> _logger;
    private readonly int _intervalSeconds;
    private readonly int _timeoutSeconds;

    public HeartbeatService(IConnectionManager connectionManager, ILogger<HeartbeatService> logger, IOptions<OpenSyncOptions> options)
    {
        _connectionManager = connectionManager;
        _logger = logger;
        _intervalSeconds = options.Value.Heartbeat.IntervalSeconds;
        _timeoutSeconds = options.Value.Heartbeat.TimeoutSeconds;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Heartbeat service started with interval {Interval}s, timeout {Timeout}s", _intervalSeconds, _timeoutSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);

            var now = DateTime.UtcNow;
            var staleConnections = _connectionManager.GetAllConnections()
                .Where(c => (now - c.LastHeartbeat).TotalSeconds > _timeoutSeconds)
                .ToList();

            foreach (var connection in staleConnections)
            {
                _connectionManager.RemoveConnection(connection.ConnectionId);
                _logger.LogWarning("Removed stale connection {ConnectionId} for identity {Identity} (no heartbeat for {Seconds}s)",
                    connection.ConnectionId, connection.Identity, (now - connection.LastHeartbeat).TotalSeconds);
            }
        }
    }
}


