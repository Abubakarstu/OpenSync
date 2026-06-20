namespace OpenSync.Api.Models.Requests;

public class SetMapItemRequest
{
    public string Key { get; set; } = string.Empty;
    public string Data { get; set; } = "{}";
    public DateTime? ExpiresAt { get; set; }
}
