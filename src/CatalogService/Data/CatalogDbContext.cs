using CatalogService.Entities;
using CatalogService.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new FloraEntityConfig());
        modelBuilder.ApplyConfiguration(new CatalogItemEntityConfig());
        modelBuilder.ApplyConfiguration(new FloraPictureEntityConfig());
    }

    public DbSet<CatalogItem> CatalogItems { get; set; }
}