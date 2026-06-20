using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenSync.Infrastructure.Services;

public class ConnectionCleanupService : BackgroundService
{
    private readonly ILogger<ConnectionCleanupService> _logger;

    public ConnectionCleanupService(ILogger<ConnectionCleanupService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Connection cleanup service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
