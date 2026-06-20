namespace OpenSync.Core.Exceptions;

public class ConflictException : SyncException
{
    public string ObjectType { get; }
    public string ObjectId { get; }
    public long ExpectedRevision { get; }
    public long CurrentRevision { get; }

    public ConflictException(string objectType, string objectId, long expectedRevision, long currentRevision)
        : base("REVISION_CONFLICT", $"Revision conflict on {objectType} '{objectId}'. Expected {expectedRevision}, found {currentRevision}.")
    {
        ObjectType = objectType;
        ObjectId = objectId;
        ExpectedRevision = expectedRevision;
        CurrentRevision = currentRevision;
    }
}
