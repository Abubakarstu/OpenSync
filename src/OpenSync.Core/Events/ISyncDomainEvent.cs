namespace OpenSync.Core.Events;

public interface ISyncDomainEvent
{
    string EventType { get; }
    DateTime Timestamp { get; }
}
