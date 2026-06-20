using Microsoft.Extensions.Logging;
using OpenSync.Infrastructure.Realtime.Transports;

namespace OpenSync.Infrastructure.Realtime.Dispatcher;

public class FanOutStrategy
{
    private readonly ITransport _transport;
    private readonly ILogger<FanOutStrategy> _logger;

    public FanOutStrategy(ITransport transport, ILogger<FanOutStrategy> logger)
    {
        _transport = transport;
        _logger = logger;
    }

    public async Task SendAsync(IReadOnlyList<string> connectionIds, string message, CancellationToken cancellationToken = default)
    {
        if (connectionIds.Count == 0) return;

        const int parallelThreshold = 50;
        if (connectionIds.Count <= parallelThreshold)
        {
            await _transport.SendManyAsync(connectionIds, message, cancellationToken);
            return;
        }

        var batches = connectionIds
            .Select((id, index) => new { id, index })
            .GroupBy(x => x.index / parallelThreshold)
            .Select(g => g.Select(x => x.id).ToList())
            .ToList();

        var tasks = batches.Select(batch =>
            _transport.SendManyAsync(batch, message, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
