using System.Text.Json;

namespace OpenSync.Sdk.Auth;

public class TokenProvider : ITokenProvider
{
    private readonly string _apiKey;
    private string? _cachedToken;
    private DateTime _expiresAt;

    public TokenProvider(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _expiresAt)
            return _cachedToken;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
        var response = await client.PostAsync("http://localhost:5000/api/v1/sync/tokens",
            new StringContent("{}", System.Text.Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        _cachedToken = doc.RootElement.GetProperty("data").GetProperty("token").GetString();
        _expiresAt = DateTime.UtcNow.AddMinutes(55);

        return _cachedToken!;
    }
}
