using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CompletedOrderConfiguration : IEntityTypeConfiguration<CompletedOrder>
{
    public void Configure(EntityTypeBuilder<CompletedOrder> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
