namespace OpenSync.Infrastructure.Webhooks;

public interface IWebhookDispatcher
{
    Task DispatchAsync(string url, string payload, string? secret = null, CancellationToken cancellationToken = default);
}
