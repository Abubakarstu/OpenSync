using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class ObjectCreatedEvent : BaseDomainEvent
{
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }
    public string? Data { get; }

    public ObjectCreatedEvent(SyncObjectType objectType, string objectId, string? data = null)
        : base("object_created")
    {
        ObjectType = objectType;
        ObjectId = objectId;
        Data = data;
    }
}
