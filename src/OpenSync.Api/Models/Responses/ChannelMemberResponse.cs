using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class ChannelMemberResponse
{
    public Guid Id { get; set; }
    public Guid ChannelId { get; set; }
    public string Identity { get; set; } = string.Empty;
    public string? Metadata { get; set; }
    public DateTime LastSeenAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static ChannelMemberResponse FromEntity(ChannelMember member) => new()
    {
        Id = member.Id,
        ChannelId = member.ChannelId,
        Identity = member.Identity,
        Metadata = member.Metadata?.Raw,
        LastSeenAt = member.LastSeenAt,
        CreatedAt = member.CreatedAt,
        UpdatedAt = member.UpdatedAt
    };
}
