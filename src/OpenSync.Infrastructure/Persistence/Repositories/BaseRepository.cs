using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Interfaces.Repositories;
using System.Linq.Expressions;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly OpenSyncDbContext Context;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(OpenSyncDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync(new object[] { id }, cancellationToken);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbSet.ToListAsync(cancellationToken);

    public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await DbSet.Where(predicate).ToListAsync(cancellationToken);

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(predicate, cancellationToken);

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        => predicate == null ? await DbSet.CountAsync(cancellationToken) : await DbSet.CountAsync(predicate, cancellationToken);

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entry = await DbSet.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await Context.SaveChangesAsync(cancellationToken);
}
