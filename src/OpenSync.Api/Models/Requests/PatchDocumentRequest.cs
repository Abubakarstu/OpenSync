namespace OpenSync.Api.Models.Requests;

public class PatchDocumentRequest
{
    public string Data { get; set; } = "{}";
    public long? ExpectedRevision { get; set; }
}
