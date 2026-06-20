using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class DocumentResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string? UniqueName { get; set; }
    public string? Data { get; set; }
    public long Revision { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static DocumentResponse FromEntity(SyncDocument doc) => new()
    {
        Id = doc.Id,
        ServiceId = doc.ServiceId,
        UniqueName = doc.UniqueName,
        Data = doc.Data?.Raw,
        Revision = doc.Revision,
        ExpiresAt = doc.ExpiresAt,
        CreatedAt = doc.CreatedAt,
        UpdatedAt = doc.UpdatedAt
    };
}
