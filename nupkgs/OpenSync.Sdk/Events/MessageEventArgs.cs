namespace OpenSync.Sdk.Events;

public class MessageEventArgs : EventArgs
{
    public string? Data { get; }
    public MessageEventArgs(string? data) => Data = data;
}
