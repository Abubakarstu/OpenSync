using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class ListItemResponse
{
    public Guid Id { get; set; }
    public Guid ListId { get; set; }
    public int Index { get; set; }
    public string? Data { get; set; }
    public long Revision { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static ListItemResponse FromEntity(SyncListItem item) => new()
    {
        Id = item.Id,
        ListId = item.ListId,
        Index = item.Index,
        Data = item.Data?.Raw,
        Revision = item.Revision,
        ExpiresAt = item.ExpiresAt,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
    };
}
