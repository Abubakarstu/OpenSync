using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class ObjectRemovedEvent : BaseDomainEvent
{
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }

    public ObjectRemovedEvent(SyncObjectType objectType, string objectId)
        : base("object_removed")
    {
        ObjectType = objectType;
        ObjectId = objectId;
    }
}
