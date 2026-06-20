namespace OpenSync.Core.Events;

public abstract class BaseDomainEvent : ISyncDomainEvent
{
    public string EventType { get; }
    public DateTime Timestamp { get; }

    protected BaseDomainEvent(string eventType)
    {
        EventType = eventType;
        Timestamp = DateTime.UtcNow;
    }
}
