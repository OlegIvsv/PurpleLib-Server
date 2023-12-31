using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Entities.Configurations;

public class FloraEntityConfig : IEntityTypeConfiguration<Flora>
{
    public void Configure(EntityTypeBuilder<Flora> builder)
    {
        builder.HasKey(f => f.Id);
        builder.HasMany(f => f.Pictures)
            .WithOne(p => p.Flora)
            .HasForeignKey(p => p.FloraId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}