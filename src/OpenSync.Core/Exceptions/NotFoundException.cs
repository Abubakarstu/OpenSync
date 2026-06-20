namespace OpenSync.Core.Exceptions;

public class NotFoundException : SyncException
{
    public string ObjectType { get; }
    public string ObjectId { get; }

    public NotFoundException(string objectType, string objectId)
        : base("NOT_FOUND", $"{objectType} '{objectId}' not found.")
    {
        ObjectType = objectType;
        ObjectId = objectId;
    }
}
