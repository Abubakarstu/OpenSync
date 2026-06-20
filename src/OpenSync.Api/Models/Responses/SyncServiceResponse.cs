using OpenSync.Core.Entities;

namespace OpenSync.Api.Models.Responses;

public class SyncServiceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? WebhookUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static SyncServiceResponse FromEntity(SyncService service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        Description = service.Description,
        WebhookUrl = service.WebhookUrl,
        IsActive = service.IsActive,
        CreatedAt = service.CreatedAt,
        UpdatedAt = service.UpdatedAt
    };
}
