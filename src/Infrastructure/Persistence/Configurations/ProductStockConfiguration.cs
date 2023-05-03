using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Units)
            .IsRequired();
    }
}
