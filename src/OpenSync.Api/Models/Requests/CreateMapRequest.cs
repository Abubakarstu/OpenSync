namespace OpenSync.Api.Models.Requests;

public class CreateMapRequest
{
    public string? UniqueName { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
