namespace OpenSync.Core.Exceptions;

public class DuplicateException : SyncException
{
    public string ObjectType { get; }
    public string? Key { get; }

    public DuplicateException(string objectType, string? key = null)
        : base("DUPLICATE", $"{objectType} already exists{ (key != null ? $" with key '{key}'" : "") }.")
    {
        ObjectType = objectType;
        Key = key;
    }
}
