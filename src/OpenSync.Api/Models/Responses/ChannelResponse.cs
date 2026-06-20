using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class ChannelResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string? UniqueName { get; set; }
    public int MemberCount { get; set; }
    public string? ChannelType { get; set; }
    public bool? IsPrivate { get; set; }
    public int? MaxMembers { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static ChannelResponse FromEntity(SyncChannel channel) => new()
    {
        Id = channel.Id,
        ServiceId = channel.ServiceId,
        UniqueName = channel.UniqueName,
        MemberCount = channel.MemberCount,
        ChannelType = channel.Attributes?.Type,
        IsPrivate = channel.Attributes?.IsPrivate,
        MaxMembers = channel.Attributes?.MaxMembers,
        ExpiresAt = channel.ExpiresAt,
        CreatedAt = channel.CreatedAt,
        UpdatedAt = channel.UpdatedAt
    };
}
