using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenSync.Infrastructure.Configuration;
using StackExchange.Redis;

namespace OpenSync.Infrastructure.Realtime.Backplane;

public class RedisBackplane : IBackplane, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;
    private readonly string _channelPrefix;
    private readonly ILogger<RedisBackplane> _logger;
    private bool _disposed;

    public RedisBackplane(IOptions<OpenSyncOptions> options, ILogger<RedisBackplane> logger)
    {
        _logger = logger;
        var redisOptions = options.Value.Backplane?.Redis;
        var connectionString = redisOptions?.ConnectionString ?? "localhost:6379";
        _channelPrefix = redisOptions?.ChannelPrefix ?? "opensync:";

        _redis = ConnectionMultiplexer.Connect(connectionString);
        _subscriber = _redis.GetSubscriber();
        _logger.LogInformation("Redis backplane connected to {ConnectionString}", connectionString);
    }

    public async Task PublishAsync(string channel, string message, CancellationToken cancellationToken = default)
    {
        var redisChannel = new RedisChannel($"{_channelPrefix}{channel}", RedisChannel.PatternMode.Literal);
        await _subscriber.PublishAsync(redisChannel, message);
    }

    public async Task SubscribeAsync(string channel, Func<string, Task> handler, CancellationToken cancellationToken = default)
    {
        var redisChannel = new RedisChannel($"{_channelPrefix}{channel}", RedisChannel.PatternMode.Literal);
        await _subscriber.SubscribeAsync(redisChannel, async (ch, message) =>
        {
            try
            {
                await handler(message!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis backplane handler error on channel {Channel}", channel);
            }
        });
    }

    public async Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        var redisChannel = new RedisChannel($"{_channelPrefix}{channel}", RedisChannel.PatternMode.Literal);
        await _subscriber.UnsubscribeAsync(redisChannel);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _redis?.Dispose();
            _disposed = true;
        }
    }
}
