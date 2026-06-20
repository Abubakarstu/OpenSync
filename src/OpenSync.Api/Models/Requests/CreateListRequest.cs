namespace OpenSync.Api.Models.Requests;

public class CreateListRequest
{
    public string? UniqueName { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
