using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CompletedOrderIngredientConfiguration : IEntityTypeConfiguration<CompletedOrderIngredient>
{
    public void Configure(EntityTypeBuilder<CompletedOrderIngredient> builder)
    {
        builder.Property(t => t.CompletedOrderId)
            .IsRequired();

        builder.Property(t => t.IngredientId)
            .IsRequired();
    }
}
