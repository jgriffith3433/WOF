using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class ConsumedCookedRecipeConfiguration : IEntityTypeConfiguration<ConsumedCookedRecipe>
{
    public void Configure(EntityTypeBuilder<ConsumedCookedRecipe> builder)
    {
    }
}
