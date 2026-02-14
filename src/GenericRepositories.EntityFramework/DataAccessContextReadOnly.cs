using GenericRepositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositories.EntityFramework;

public class DataAccessContextReadOnly<TDbContext>(TDbContext dbContext) : IDataAccessContext
    where TDbContext : DbContext
{
    // ReSharper disable once MemberCanBePrivate.Global
    public TDbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Save changes is not supported in a read-only context.");
}
