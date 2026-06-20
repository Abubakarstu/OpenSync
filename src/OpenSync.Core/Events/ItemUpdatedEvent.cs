using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class ItemUpdatedEvent : BaseDomainEvent
{
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }
    public string ItemId { get; }
    public string? Data { get; }
    public long Revision { get; }

    public ItemUpdatedEvent(SyncObjectType objectType, string objectId, string itemId, long revision, string? data = null)
        : base("item_updated")
    {
        ObjectType = objectType;
        ObjectId = objectId;
        ItemId = itemId;
        Revision = revision;
        Data = data;
    }
}
