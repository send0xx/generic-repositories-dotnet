using GenericRepositories.Abstractions;
using GenericRepositories.SampleApi.Data;
using GenericRepositories.SampleApi.Infrastructure;
using GenericRepositories.SampleApi.Models;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GenericRepositoriesSampleDb"));

builder.Services.AddScoped<AppDataAccessContextReadOnly>();
builder.Services.AddScoped<AppDataAccessContext>();
builder.Services.AddScoped(typeof(IRepositoryReadOnly<>), typeof(AppRepositoryReadOnly<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(AppRepository<>));

WebApplication app = builder.Build();

await SeedAsync(app.Services);

app.MapGet("/", () => Results.Ok(new
{
    Name = "GenericRepositories.SampleApi",
    Endpoints = (string[])
    [
        "GET /api/products",
        "GET /api/products/{id}",
        "POST /api/products",
        "PUT /api/products/{id}",
        "DELETE /api/products/{id}"
    ]
}));

RouteGroupBuilder products = app.MapGroup("/api/products");

products.MapGet("/", async (IRepositoryReadOnly<Product> repository, CancellationToken cancellationToken) =>
{
    List<Product> result = await repository.ListAsync(cancellationToken);
    return Results.Ok(result);
});

products.MapGet("/{id:guid}", async (Guid id, IRepositoryReadOnly<Product> repository, CancellationToken cancellationToken) =>
{
    Product? product = await repository.GetByIdAsync(id, cancellationToken);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

products.MapPost("/", async (CreateProductRequest request, IRepository<Product> repository, CancellationToken cancellationToken) =>
{
    Product product = new()
    {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        Price = request.Price,
        CreatedAtUtc = DateTime.UtcNow
    };

    await repository.AddAsync(product, cancellationToken);
    await repository.DataAccessContext.SaveChangesAsync(cancellationToken);

    return Results.Created($"/api/products/{product.Id}", product);
});

products.MapPut("/{id:guid}",
    async (Guid id, UpdateProductRequest request, IRepository<Product> repository, CancellationToken cancellationToken) =>
    {
        Product? product = await repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return Results.NotFound();
        }

        product.Name = request.Name.Trim();
        product.Price = request.Price;

        repository.Update(product);
        await repository.DataAccessContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(product);
    });

products.MapDelete("/{id:guid}", async (Guid id, IRepository<Product> repository, CancellationToken cancellationToken) =>
{
    Product? product = await repository.GetByIdAsync(id, cancellationToken);
    if (product is null)
    {
        return Results.NotFound();
    }

    repository.Remove(product);
    await repository.DataAccessContext.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

app.Run();
return;

static async Task SeedAsync(IServiceProvider services)
{
    using IServiceScope scope = services.CreateScope();
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (await dbContext.Products.AnyAsync())
    {
        return;
    }

    dbContext.Products.AddRange(
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Keyboard",
            Price = 99.00m,
            CreatedAtUtc = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mouse",
            Price = 49.00m,
            CreatedAtUtc = DateTime.UtcNow
        });

    await dbContext.SaveChangesAsync();
}
