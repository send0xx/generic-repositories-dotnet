using GenericRepositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositories.EntityFramework;

public class DataAccessContext<TDbContext>(TDbContext dbContext) : IDataAccessContext
    where TDbContext : DbContext
{
    // ReSharper disable once MemberCanBePrivate.Global
    public TDbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        DbContext.SaveChangesAsync(cancellationToken);
}
