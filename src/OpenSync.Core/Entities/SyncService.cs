using OpenSync.Core.ValueObjects;

namespace OpenSync.Core.Entities;

public class SyncService : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? WebhookUrl { get; private set; }
    public string? WebhookSecret { get; private set; }
    public bool IsActive { get; private set; }
    public ServiceLimits? Limits { get; private set; }

    private SyncService() { }

    public SyncService(string name, string? description = null, string? webhookUrl = null, string? webhookSecret = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        WebhookUrl = webhookUrl;
        WebhookSecret = webhookSecret;
        IsActive = true;
        Limits = new ServiceLimits();
    }

    public void Update(string name, string? description, string? webhookUrl, string? webhookSecret)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        WebhookUrl = webhookUrl;
        WebhookSecret = webhookSecret;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetLimits(ServiceLimits limits)
    {
        Limits = limits;
        UpdatedAt = DateTime.UtcNow;
    }
}
