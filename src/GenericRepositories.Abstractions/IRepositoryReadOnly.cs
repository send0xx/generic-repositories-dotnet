// ReSharper disable UnusedMember.Global

using System.Linq.Expressions;

namespace GenericRepositories.Abstractions;

public interface IRepositoryReadOnly<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

    Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
