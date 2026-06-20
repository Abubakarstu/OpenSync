using OpenSync.Core.Enums;

namespace OpenSync.Core.Events;

public class ObjectUpdatedEvent : BaseDomainEvent
{
    public SyncObjectType ObjectType { get; }
    public string ObjectId { get; }
    public string? Data { get; }
    public long Revision { get; }

    public ObjectUpdatedEvent(SyncObjectType objectType, string objectId, long revision, string? data = null)
        : base("object_updated")
    {
        ObjectType = objectType;
        ObjectId = objectId;
        Revision = revision;
        Data = data;
    }
}
