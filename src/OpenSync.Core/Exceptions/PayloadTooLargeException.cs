namespace OpenSync.Core.Exceptions;

public class PayloadTooLargeException : SyncException
{
    public long ActualSize { get; }
    public long MaxSize { get; }

    public PayloadTooLargeException(long actualSize, long maxSize)
        : base("PAYLOAD_TOO_LARGE", $"Payload size {actualSize} bytes exceeds maximum {maxSize} bytes.")
    {
        ActualSize = actualSize;
        MaxSize = maxSize;
    }
}
