// ReSharper disable UnusedMember.Global

namespace GenericRepositories.Abstractions;

public interface IRepository<TEntity> : IRepositoryReadOnly<TEntity>
    where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
