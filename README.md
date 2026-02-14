# Generic Repositories for .NET

Generic repository abstractions and Entity Framework Core implementations for .NET 10.

## Features

- **Read-only and read/write repositories** — separate contracts enforce read intent at the type level
- **Unit of Work** via `IDataAccessContext` — multiple repositories share the same `DbContext`; a single `SaveChangesAsync` call persists all changes
- **No-tracking by default** — `RepositoryReadOnly` queries use `AsNoTrackingWithIdentityResolution`

## Projects

| Project | Description |
|---------|-------------|
| `GenericRepositories.Abstractions` | Interfaces: `IDataAccessContext`, `IRepositoryReadOnly<TEntity>`, `IRepository<TEntity>` |
| `GenericRepositories.EntityFramework` | EF Core implementations: `DataAccessContext`, `DataAccessContextReadOnly`, `Repository`, `RepositoryReadOnly` |
| `GenericRepositories.SampleApi` | Minimal API CRUD example with EF Core InMemory provider |

## Usage

### 1. Create app-specific wrappers

```csharp
public class AppDataAccessContext(AppDbContext dbContext)
    : DataAccessContext<AppDbContext>(dbContext);

public class AppDataAccessContextReadOnly(AppDbContext dbContext)
    : DataAccessContextReadOnly<AppDbContext>(dbContext);

public class AppRepository<TEntity>(AppDataAccessContext dataAccessContext)
    : Repository<AppDbContext, TEntity>(dataAccessContext)
    where TEntity : class;

public class AppRepositoryReadOnly<TEntity>(AppDataAccessContextReadOnly dataAccessContext)
    : RepositoryReadOnly<AppDbContext, TEntity>(dataAccessContext)
    where TEntity : class;
```

### 2. Register in DI

```csharp
builder.Services.AddDbContext<AppDbContext>(options => ...);

builder.Services.AddScoped<AppDataAccessContext>();
builder.Services.AddScoped<AppDataAccessContextReadOnly>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(AppRepository<>));
builder.Services.AddScoped(typeof(IRepositoryReadOnly<>), typeof(AppRepositoryReadOnly<>));
```

### 3. Inject and use

```csharp
// Read-only — no-tracking queries, SaveChangesAsync throws NotSupportedException
app.MapGet("/products", async (IRepositoryReadOnly<Product> repository, CancellationToken ct) =>
{
    List<Product> products = await repository.ListAsync(ct);
    return Results.Ok(products);
});

// Read/write — save via DataAccessContext (Unit of Work)
app.MapPost("/products", async (Product product, IRepository<Product> repository, CancellationToken ct) =>
{
    await repository.AddAsync(product, ct);
    await repository.DataAccessContext.SaveChangesAsync(ct);
    return Results.Created($"/products/{product.Id}", product);
});
```

## API surface

### `IDataAccessContext`

| Method | Description |
|--------|-------------|
| `SaveChangesAsync(CancellationToken)` | Persists all tracked changes. Throws `NotSupportedException` in read-only context. |

### `IRepositoryReadOnly<TEntity>`

| Member | Description |
|--------|-------------|
| `DataAccessContext` | The shared `IDataAccessContext` instance |
| `GetByIdAsync<TId>(...)` | Find entity by primary key |
| `FirstOrDefaultAsync(...)` | First entity, with optional predicate |
| `SingleOrDefaultAsync(...)` | Single entity, with optional predicate |
| `ListAsync(...)` | All entities, with optional predicate |
| `CountAsync(...)` | Count entities, with optional predicate |
| `AnyAsync(...)` | Check existence, with optional predicate |

### `IRepository<TEntity>` (extends `IRepositoryReadOnly<TEntity>`)

| Method | Description |
|--------|-------------|
| `AddAsync(...)` | Add a single entity |
| `AddRangeAsync(...)` | Add multiple entities |
| `Update(...)` | Mark entity as modified |
| `UpdateRange(...)` | Mark multiple entities as modified |
| `Remove(...)` | Mark entity for deletion |
| `RemoveRange(...)` | Mark multiple entities for deletion |

## Build

```bash
dotnet build GenericRepositories.slnx
```

## Run sample API

```bash
dotnet run --project samples/GenericRepositories.SampleApi
```

Default local URL: `http://localhost:5116`

## Test with HTTP file

Use `samples/GenericRepositories.SampleApi/GenericRepositories.SampleApi.http` — it contains a full CRUD flow (list, create, get by id, update, delete).
