using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Entities.Configurations;

public class FloraPictureEntityConfig : IEntityTypeConfiguration<FloraPicture>
{
    public void Configure(EntityTypeBuilder<FloraPicture> builder)
    {
        builder.HasKey(p => p.Id);
    }
}