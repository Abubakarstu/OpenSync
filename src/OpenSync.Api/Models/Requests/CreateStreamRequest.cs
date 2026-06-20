namespace OpenSync.Api.Models.Requests;

public class CreateStreamRequest
{
    public string? UniqueName { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
