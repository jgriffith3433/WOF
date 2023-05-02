using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CompletedOrderConfiguration : IEntityTypeConfiguration<CompletedOrder>
{
    public void Configure(EntityTypeBuilder<CompletedOrder> builder)
    {
        builder.Property(t => t.UserImport)
            .IsRequired();

        builder.HasMany(left => left.Ingredients).WithMany(right => right.CompletedOrders).UsingEntity("CompletedOrderIngredient", typeof(Dictionary<string, object>),
            right => right.HasOne(typeof(Ingredient)).WithMany().HasForeignKey("IngredientId"),
            left => left.HasOne(typeof(CompletedOrder)).WithMany().HasForeignKey("CompletedOrderId"),
            join => join.ToTable("CompletedOrderIngredients")
        );
    }
}
