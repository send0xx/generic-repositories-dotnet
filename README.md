# Generic repositories

Generic repository abstractions and Entity Framework Core implementations for .NET 10.

The solution separates:
- read-only repository contract and implementation
- read/write repository contract and implementation

## Projects

- `src/GenericRepositories.Abstractions`
  - repository interfaces
  - `IRepositoryReadOnly<TEntity>`
  - `IRepository<TEntity>`
- `src/GenericRepositories.EntityFramework`
  - EF Core base implementations
  - `RepositoryReadOnly<TDbContext, TEntity>`
  - `Repository<TDbContext, TEntity>`
- `samples/GenericRepositories.SampleApi`
  - minimal API CRUD example using both repository types
  - EF Core InMemory provider

## Key API surface

Read-only (`IRepositoryReadOnly<TEntity>`):
- `GetByIdAsync<TId>(...)`
- `FirstOrDefaultAsync(...)` overloads
- `SingleOrDefaultAsync(...)` overloads
- `ListAsync(...)` overloads
- `CountAsync(...)` overloads
- `AnyAsync(...)` overloads

Read/write (`IRepository<TEntity>`):
- `AddAsync(...)`
- `AddRangeAsync(...)`
- `Update(...)`
- `UpdateRange(...)`
- `Remove(...)`
- `RemoveRange(...)`
- `SaveChangesAsync(...)`

## Build

```bash
dotnet build GenericRepositories.slnx
```

## Run sample API

```bash
dotnet run --project samples/GenericRepositories.SampleApi
```

Default local URL (HTTP profile):
- `http://localhost:5116`

## Test with HTTP file

Use:
- `samples/GenericRepositories.SampleApi/GenericRepositories.SampleApi.http`

It contains a full CRUD flow:
- list products
- create product
- get by id
- update
- delete
