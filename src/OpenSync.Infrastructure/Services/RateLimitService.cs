using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace OpenSync.Infrastructure.Services;

public class RateLimitService
{
    private readonly ConcurrentDictionary<string, SlidingWindowCounter> _counters = new();
    private readonly int _defaultLimitPerSecond;
    private readonly ILogger<RateLimitService>? _logger;

    public RateLimitService(int defaultLimitPerSecond = 100, ILogger<RateLimitService>? logger = null)
    {
        _defaultLimitPerSecond = defaultLimitPerSecond;
        _logger = logger;
    }

    public bool IsAllowed(string key, int limitPerSecond, out int remaining, out long resetTimestampMs)
    {
        var counter = _counters.GetOrAdd(key, _ => new SlidingWindowCounter(limitPerSecond));
        var (allowed, remaining_, resetMs) = counter.TryConsume();
        remaining = remaining_;
        resetTimestampMs = resetMs;

        if (!allowed)
        {
            _logger?.LogWarning("Rate limit exceeded for key {Key}: {Limit} requests/second", key, limitPerSecond);
        }

        return allowed;
    }

    public bool IsAllowed(string key, out int remaining, out long resetTimestampMs)
        => IsAllowed(key, _defaultLimitPerSecond, out remaining, out resetTimestampMs);

    public void Reset(string key)
    {
        _counters.TryRemove(key, out _);
    }

    private class SlidingWindowCounter
    {
        private readonly int _limit;
        private readonly long _windowMs = 1000;
        private readonly object _lock = new();
        private readonly Queue<long> _timestamps = new();

        public SlidingWindowCounter(int limit)
        {
            _limit = limit;
        }

        public (bool Allowed, int Remaining, long ResetMs) TryConsume()
        {
            lock (_lock)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var windowStart = now - _windowMs;

                while (_timestamps.Count > 0 && _timestamps.Peek() < windowStart)
                    _timestamps.Dequeue();

                if (_timestamps.Count >= _limit)
                {
                    var oldest = _timestamps.Peek();
                    var resetMs = oldest + _windowMs;
                    return (false, 0, resetMs);
                }

                _timestamps.Enqueue(now);
                var remaining = _limit - _timestamps.Count;
                return (true, remaining, now + _windowMs);
            }
        }
    }
}
