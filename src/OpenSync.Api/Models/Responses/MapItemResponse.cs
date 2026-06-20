using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class MapItemResponse
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Data { get; set; }
    public long Revision { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static MapItemResponse FromEntity(SyncMapItem item) => new()
    {
        Id = item.Id,
        MapId = item.MapId,
        Key = item.Key,
        Data = item.Data?.Raw,
        Revision = item.Revision,
        ExpiresAt = item.ExpiresAt,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
    };
}
