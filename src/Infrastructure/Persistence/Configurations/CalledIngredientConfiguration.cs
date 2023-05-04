using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CalledIngredientConfiguration : IEntityTypeConfiguration<CalledIngredient>
{
    public void Configure(EntityTypeBuilder<CalledIngredient> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
