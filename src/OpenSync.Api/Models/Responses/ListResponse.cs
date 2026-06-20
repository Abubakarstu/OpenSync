using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class ListResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string? UniqueName { get; set; }
    public long Revision { get; set; }
    public int ItemCount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static ListResponse FromEntity(SyncList list) => new()
    {
        Id = list.Id,
        ServiceId = list.ServiceId,
        UniqueName = list.UniqueName,
        Revision = list.Revision,
        ItemCount = list.ItemCount,
        ExpiresAt = list.ExpiresAt,
        CreatedAt = list.CreatedAt,
        UpdatedAt = list.UpdatedAt
    };
}
