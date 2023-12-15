using Amazon.Runtime.Internal;
using CatalogService.Entities;
using CatalogService.Entities.Configurations;
using MassTransit;
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
        // Entities
        modelBuilder.ApplyConfiguration(new FloraEntityConfig());
        modelBuilder.ApplyConfiguration(new CatalogItemEntityConfig());
        modelBuilder.ApplyConfiguration(new FloraPictureEntityConfig());
        // Outbox pattern configuration for MassTransit
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    public DbSet<CatalogItem> CatalogItems { get; set; }
}