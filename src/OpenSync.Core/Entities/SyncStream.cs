namespace OpenSync.Core.Entities;

public class SyncStream : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public string? UniqueName { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private SyncStream() { }

    public SyncStream(Guid serviceId, string? uniqueName = null, DateTime? expiresAt = null)
    {
        ServiceId = serviceId;
        UniqueName = uniqueName;
        ExpiresAt = expiresAt;
    }
}
