using GenericRepositories.EntityFramework;
using GenericRepositories.SampleApi.Data;

namespace GenericRepositories.SampleApi.Infrastructure;

public class AppDataAccessContextReadOnly(AppDbContext dbContext) : DataAccessContextReadOnly<AppDbContext>(dbContext);

public class AppDataAccessContext(AppDbContext dbContext) : DataAccessContext<AppDbContext>(dbContext);

public class AppRepositoryReadOnly<TEntity>(AppDataAccessContextReadOnly dataAccessContext)
    : RepositoryReadOnly<AppDbContext, TEntity>(dataAccessContext)
    where TEntity : class;

public class AppRepository<TEntity>(AppDataAccessContext dataAccessContext)
    : Repository<AppDbContext, TEntity>(dataAccessContext)
    where TEntity : class;
