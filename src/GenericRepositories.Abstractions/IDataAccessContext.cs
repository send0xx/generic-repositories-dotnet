namespace GenericRepositories.Abstractions;

public interface IDataAccessContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
