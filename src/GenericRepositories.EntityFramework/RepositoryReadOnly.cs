using System.Linq.Expressions;
using GenericRepositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositories.EntityFramework;

public class RepositoryReadOnly<TDbContext, TEntity>(DataAccessContextReadOnly<TDbContext> dataAccessContext)
    : IRepositoryReadOnly<TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    public IDataAccessContext DataAccessContext => dataAccessContext;

    // ReSharper disable once MemberCanBePrivate.Global
    protected TDbContext DbContext => dataAccessContext.DbContext;

    // Warning: DbSet queries are tracked by default. Use Query() instead to ensure no-tracking behavior, or call AsNoTracking/AsNoTrackingWithIdentityResolution.
    // ReSharper disable once MemberCanBePrivate.Global
    protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual IQueryable<TEntity> Query() => DbSet.AsNoTrackingWithIdentityResolution();

    public virtual async Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : notnull
    {
        TEntity? entity = await DbSet.FindAsync([id], cancellationToken);

        if (entity is not null)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public virtual async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default) =>
        await Query().ToListAsync(cancellationToken);

    public virtual async Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await Query().Where(predicate).ToListAsync(cancellationToken);

    public virtual Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default) =>
        Query().FirstOrDefaultAsync(cancellationToken);

    public virtual Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Query().FirstOrDefaultAsync(predicate, cancellationToken);

    public virtual Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default) =>
        Query().SingleOrDefaultAsync(cancellationToken);

    public virtual Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Query().SingleOrDefaultAsync(predicate, cancellationToken);

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Query().CountAsync(cancellationToken);

    public virtual Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => Query().CountAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        Query().AnyAsync(cancellationToken);

    public virtual Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Query().AnyAsync(predicate, cancellationToken);
}
