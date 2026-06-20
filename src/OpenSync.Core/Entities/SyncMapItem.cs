using OpenSync.Core.ValueObjects;

namespace OpenSync.Core.Entities;

public class SyncMapItem : BaseEntity
{
    public Guid MapId { get; private set; }
    public string Key { get; private set; } = null!;
    public JsonData Data { get; private set; } = null!;
    public long Revision { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private SyncMapItem() { }

    public SyncMapItem(Guid mapId, string key, JsonData data, DateTime? expiresAt = null)
    {
        MapId = mapId;
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Revision = 0;
        ExpiresAt = expiresAt;
    }

    public void UpdateData(JsonData newData)
    {
        Data = newData ?? throw new ArgumentNullException(nameof(newData));
        Revision++;
        UpdatedAt = DateTime.UtcNow;
    }
}
