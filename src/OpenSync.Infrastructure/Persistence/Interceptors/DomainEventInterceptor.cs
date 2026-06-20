using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpenSync.Core.Entities;
using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Infrastructure.Persistence.Interceptors;

public class DomainEventInterceptor : SaveChangesInterceptor
{
    private readonly IEventPublisher _eventPublisher;

    public DomainEventInterceptor(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchEvents(DbContext? context)
    {
        if (context == null) return;

        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await _eventPublisher.PublishAsync(domainEvent);
    }
}
