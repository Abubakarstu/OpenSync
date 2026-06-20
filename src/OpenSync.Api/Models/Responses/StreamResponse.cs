using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class StreamResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string? UniqueName { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static StreamResponse FromEntity(SyncStream stream) => new()
    {
        Id = stream.Id,
        ServiceId = stream.ServiceId,
        UniqueName = stream.UniqueName,
        ExpiresAt = stream.ExpiresAt,
        CreatedAt = stream.CreatedAt,
        UpdatedAt = stream.UpdatedAt
    };
}
