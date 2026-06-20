namespace OpenSync.Core.Exceptions;

public class SyncException : Exception
{
    public string Code { get; }

    public SyncException(string code, string message) : base(message)
    {
        Code = code;
    }

    public SyncException(string code, string message, Exception inner) : base(message, inner)
    {
        Code = code;
    }
}
