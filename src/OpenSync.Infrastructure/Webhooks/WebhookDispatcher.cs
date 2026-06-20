using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OpenSync.Infrastructure.Webhooks;

public class WebhookDispatcher : IWebhookDispatcher
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WebhookDispatcher> _logger;

    public WebhookDispatcher(HttpClient httpClient, ILogger<WebhookDispatcher> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task DispatchAsync(string url, string payload, string? secret = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(secret))
            {
                var signature = ComputeSignature(payload, secret);
                content.Headers.Add("X-Sync-Signature", signature);
            }

            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Webhook to {Url} returned {StatusCode}", url, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to dispatch webhook to {Url}", url);
        }
    }

    private static string ComputeSignature(string payload, string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToBase64String(hash);
    }
}
