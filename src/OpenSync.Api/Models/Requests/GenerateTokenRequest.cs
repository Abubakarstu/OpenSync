namespace OpenSync.Api.Models.Requests;

public class GenerateTokenRequest
{
    public string Identity { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public Dictionary<string, string[]>? Permissions { get; set; }
    public int ExpirationMinutes { get; set; } = 60;
}
