namespace OpenSync.Sdk.Events;

public class SyncEventArgs : EventArgs
{
    public string Event { get; set; } = string.Empty;
    public string ObjectType { get; set; } = string.Empty;
    public string ObjectId { get; set; } = string.Empty;
    public string? Data { get; set; }
    public DateTime Timestamp { get; set; }
}
