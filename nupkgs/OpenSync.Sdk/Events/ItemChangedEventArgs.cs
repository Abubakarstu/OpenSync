namespace OpenSync.Sdk.Events;

public class ItemChangedEventArgs : EventArgs
{
    public string? Data { get; }
    public ItemChangedEventArgs(string? data) => Data = data;
}
