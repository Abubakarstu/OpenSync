using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace OpenSync.Infrastructure.Realtime.Backplane;

public class InProcessBackplane : IBackplane, IDisposable
{
    private readonly ConcurrentDictionary<string, Channel<string>> _channels = new();
    private readonly ConcurrentDictionary<string, List<Func<string, Task>>> _handlers = new();
    private readonly ILogger<InProcessBackplane> _logger;
    private readonly CancellationTokenSource _cts = new();

    public InProcessBackplane(ILogger<InProcessBackplane> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync(string channel, string message, CancellationToken cancellationToken = default)
    {
        if (_channels.TryGetValue(channel, out var ch))
        {
            await ch.Writer.WriteAsync(message, cancellationToken);
        }
    }

    public Task SubscribeAsync(string channel, Func<string, Task> handler, CancellationToken cancellationToken = default)
    {
        _handlers.AddOrUpdate(channel,
            _ => new List<Func<string, Task>> { handler },
            (_, list) => { list.Add(handler); return list; });

        var ch = _channels.GetOrAdd(channel, _ => Channel.CreateUnbounded<string>(new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = false
        }));

        _ = Task.Run(async () =>
        {
            var reader = ch.Reader;
            try
            {
                await foreach (var message in reader.ReadAllAsync(_cts.Token))
                {
                    if (_handlers.TryGetValue(channel, out var handlers))
                    {
                        foreach (var h in handlers)
                        {
                            try { await h(message); }
                            catch (Exception ex) { _logger.LogError(ex, "Backplane handler error on channel {Channel}", channel); }
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        _handlers.TryRemove(channel, out _);
        _channels.TryRemove(channel, out _);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
