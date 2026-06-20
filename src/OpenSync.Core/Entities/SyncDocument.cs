using OpenSync.Core.ValueObjects;

namespace OpenSync.Core.Entities;

public class SyncDocument : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public string? UniqueName { get; private set; }
    public JsonData Data { get; private set; } = null!;
    public long Revision { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private SyncDocument() { }

    public SyncDocument(Guid serviceId, JsonData data, string? uniqueName = null, DateTime? expiresAt = null)
    {
        ServiceId = serviceId;
        UniqueName = uniqueName;
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Revision = 0;
        ExpiresAt = expiresAt;
    }

    public void UpdateData(JsonData newData, long? expectedRevision = null)
    {
        if (expectedRevision.HasValue && expectedRevision.Value != Revision)
            throw new Exceptions.ConflictException("Document", Id.ToString(), expectedRevision.Value, Revision);
        
        Data = newData ?? throw new ArgumentNullException(nameof(newData));
        Revision++;
        UpdatedAt = DateTime.UtcNow;
    }
}
