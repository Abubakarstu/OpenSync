using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class MapResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string? UniqueName { get; set; }
    public long Revision { get; set; }
    public int ItemCount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static MapResponse FromEntity(SyncMap map) => new()
    {
        Id = map.Id,
        ServiceId = map.ServiceId,
        UniqueName = map.UniqueName,
        Revision = map.Revision,
        ItemCount = map.ItemCount,
        ExpiresAt = map.ExpiresAt,
        CreatedAt = map.CreatedAt,
        UpdatedAt = map.UpdatedAt
    };
}
