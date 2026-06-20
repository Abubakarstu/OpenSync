namespace OpenSync.Api.Models.Requests;

public class CreateChannelRequest
{
    public string? UniqueName { get; set; }
    public string? ChannelType { get; set; }
    public bool? IsPrivate { get; set; }
    public int? MaxMembers { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
