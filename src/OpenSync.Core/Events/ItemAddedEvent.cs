using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class ItemAddedEvent : BaseDomainEvent
{
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }
    public string ItemId { get; }
    public int Index { get; }
    public string? Data { get; }

    public ItemAddedEvent(SyncObjectType objectType, string objectId, string itemId, int index, string? data = null)
        : base("item_added")
    {
        ObjectType = objectType;
        ObjectId = objectId;
        ItemId = itemId;
        Index = index;
        Data = data;
    }
}
