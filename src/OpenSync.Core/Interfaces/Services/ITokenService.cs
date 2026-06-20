namespace OpenSync.Core.Interfaces.Services;

public class TokenPayload
{
    public string Identity { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public Dictionary<string, string[]> Permissions { get; set; } = new();
    public DateTime ExpiresAt { get; set; }
}

public interface ITokenService
{
    string GenerateToken(TokenPayload payload, string secretKey);
    TokenPayload? ValidateToken(string token, string secretKey);
}
