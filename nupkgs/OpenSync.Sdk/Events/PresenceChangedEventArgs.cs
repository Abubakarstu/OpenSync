namespace OpenSync.Sdk.Events;

public class PresenceChangedEventArgs : EventArgs
{
    public string? Data { get; }
    public PresenceChangedEventArgs(string? data) => Data = data;
}
