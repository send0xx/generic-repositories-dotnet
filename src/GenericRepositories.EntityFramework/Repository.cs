using System.Linq.Expressions;
using GenericRepositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GenericRepositories.EntityFramework;

public class Repository<TDbContext, TEntity>(DataAccessContext<TDbContext> dataAccessContext)
    : IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    public IDataAccessContext DataAccessContext => dataAccessContext;

    // ReSharper disable once MemberCanBePrivate.Global
    protected TDbContext DbContext => dataAccessContext.DbContext;

    // ReSharper disable once MemberCanBePrivate.Global
    protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual IQueryable<TEntity> Query() => DbSet;

    public virtual Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : notnull =>
        DbSet.FindAsync([id], cancellationToken).AsTask();

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
        CancellationToken cancellationToken = default) =>
        Query().CountAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        Query().AnyAsync(cancellationToken);

    public virtual Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Query().AnyAsync(predicate, cancellationToken);

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        EntityEntry<TEntity> entry = await DbSet.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        DbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);

    public void Remove(TEntity entity) => DbSet.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
}
