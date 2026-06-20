using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class ItemRemovedEvent : BaseDomainEvent
{
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }
    public string ItemId { get; }
    public int Index { get; }

    public ItemRemovedEvent(SyncObjectType objectType, string objectId, string itemId, int index)
        : base("item_removed")
    {
        ObjectType = objectType;
        ObjectId = objectId;
        ItemId = itemId;
        Index = index;
    }
}
