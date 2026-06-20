using OpenSync.Core.ValueObjects;

namespace OpenSync.Core.Entities;

public class SyncList : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public string? UniqueName { get; private set; }
    public long Revision { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public int ItemCount { get; private set; }

    private SyncList() { }

    public SyncList(Guid serviceId, string? uniqueName = null, DateTime? expiresAt = null)
    {
        ServiceId = serviceId;
        UniqueName = uniqueName;
        Revision = 0;
        ItemCount = 0;
        ExpiresAt = expiresAt;
    }

    public void IncrementRevision()
    {
        Revision++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AdjustItemCount(int delta)
    {
        ItemCount += delta;
        if (ItemCount < 0) ItemCount = 0;
    }
}
