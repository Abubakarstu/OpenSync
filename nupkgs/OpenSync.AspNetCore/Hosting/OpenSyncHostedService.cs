using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenSync.AspNetCore.Hosting;

public class OpenSyncHostedService : IHostedService
{
    private readonly ILogger<OpenSyncHostedService> _logger;

    public OpenSyncHostedService(ILogger<OpenSyncHostedService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("OpenSync hosted service started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("OpenSync hosted service stopped");
        return Task.CompletedTask;
    }
}
