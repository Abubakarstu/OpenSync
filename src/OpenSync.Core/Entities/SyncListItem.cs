using OpenSync.Core.ValueObjects;

namespace OpenSync.Core.Entities;

public class SyncListItem : BaseEntity
{
    public Guid ListId { get; private set; }
    public int Index { get; private set; }
    public JsonData Data { get; private set; } = null!;
    public long Revision { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private SyncListItem() { }

    public SyncListItem(Guid listId, int index, JsonData data, DateTime? expiresAt = null)
    {
        ListId = listId;
        Index = index;
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Revision = 0;
        ExpiresAt = expiresAt;
    }

    public void UpdateData(JsonData newData, long? expectedRevision = null)
    {
        if (expectedRevision.HasValue && expectedRevision.Value != Revision)
            throw new Exceptions.ConflictException("ListItem", Id.ToString(), expectedRevision.Value, Revision);
        
        Data = newData ?? throw new ArgumentNullException(nameof(newData));
        Revision++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetIndex(int newIndex)
    {
        Index = newIndex;
        UpdatedAt = DateTime.UtcNow;
    }
}
