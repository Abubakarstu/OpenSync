namespace OpenSync.Api.Models.Requests;

public class CreateDocumentRequest
{
    public string? UniqueName { get; set; }
    public string Data { get; set; } = "{}";
    public long? ExpectedRevision { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
