namespace OpenSync.Api.Models.Requests;

public class AddListItemRequest
{
    public string Data { get; set; } = "{}";
    public DateTime? ExpiresAt { get; set; }
}
