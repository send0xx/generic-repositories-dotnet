using GenericRepositories.EntityFramework;
using GenericRepositories.SampleApi.Data;

namespace GenericRepositories.SampleApi.Infrastructure;

public class AppRepositoryReadOnly<TEntity>(AppDbContext dbContext) : RepositoryReadOnly<AppDbContext, TEntity>(dbContext)
    where TEntity : class;

public class AppRepository<TEntity>(AppDbContext dbContext) : Repository<AppDbContext, TEntity>(dbContext)
    where TEntity : class;
