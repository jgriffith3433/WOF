using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CompletedOrderProductConfiguration : IEntityTypeConfiguration<CompletedOrderProduct>
{
    public void Configure(EntityTypeBuilder<CompletedOrderProduct> builder)
    {

    }
}
