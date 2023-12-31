using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Entities.Configurations;

public class CatalogItemEntityConfig : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasOne(i => i.Flora)
            .WithOne(f => f.CatalogItem)
            .HasForeignKey<CatalogItem>(i => i.FloraId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}