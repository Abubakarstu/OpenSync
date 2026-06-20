using Microsoft.Extensions.Logging;

namespace OpenSync.Infrastructure.Webhooks;

public class WebhookRetryPolicy
{
    private readonly int _maxRetries;
    private readonly ILogger<WebhookRetryPolicy>? _logger;

    public WebhookRetryPolicy(int maxRetries = 3, ILogger<WebhookRetryPolicy>? logger = null)
    {
        _maxRetries = maxRetries;
        _logger = logger;
    }

    public async Task<T> ExecuteAsync<T>(Func<int, Task<T>> action, CancellationToken cancellationToken = default)
    {
        int retryCount = 0;

        while (true)
        {
            try
            {
                return await action(retryCount);
            }
            catch (Exception ex) when (retryCount < _maxRetries && !cancellationToken.IsCancellationRequested)
            {
                retryCount++;
                var delayMs = Math.Min(1000 * Math.Pow(2, retryCount), 30000);
                _logger?.LogWarning(ex, "Webhook attempt {Retry}/{MaxRetries} failed, retrying in {DelayMs}ms", retryCount, _maxRetries, delayMs);
                await Task.Delay((int)delayMs, cancellationToken);
            }
        }
    }
}
