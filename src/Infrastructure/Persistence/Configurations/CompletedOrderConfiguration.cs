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

        builder.HasMany(left => left.Products).WithMany(right => right.CompletedOrders).UsingEntity("CompletedOrderProduct", typeof(Dictionary<string, object>),
            right => right.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId"),
            left => left.HasOne(typeof(CompletedOrder)).WithMany().HasForeignKey("CompletedOrderId"),
            join => join.ToTable("CompletedOrderProducts")
        );
    }
}
