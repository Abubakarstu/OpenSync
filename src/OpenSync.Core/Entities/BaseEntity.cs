using OpenSync.Core.Events;

namespace OpenSync.Core.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }

    private readonly List<ISyncDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<ISyncDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(ISyncDomainEvent eventItem) => _domainEvents.Add(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
