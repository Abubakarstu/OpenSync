namespace OpenSync.Api.Models.Requests;

public class JoinChannelRequest
{
    public string Identity { get; set; } = string.Empty;
    public string? Metadata { get; set; }
}
