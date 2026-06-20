namespace OpenSync.Api.Models.Requests;

public class UpdateDocumentRequest
{
    public string Data { get; set; } = "{}";
    public long? ExpectedRevision { get; set; }
}
