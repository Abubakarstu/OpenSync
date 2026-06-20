using System.Security.Cryptography;
using System.Text;

namespace OpenSync.Infrastructure.Webhooks;

public class WebhookSigningService
{
    public string Sign(string payload, string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToBase64String(hash);
    }

    public bool Verify(string payload, string signature, string secret)
    {
        var expected = Sign(payload, secret);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(signature));
    }
}
