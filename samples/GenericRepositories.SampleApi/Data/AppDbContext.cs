using GenericRepositories.SampleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositories.SampleApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
