using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CookedRecipeCalledIngredientConfiguration : IEntityTypeConfiguration<CookedRecipeCalledIngredient>
{
    public void Configure(EntityTypeBuilder<CookedRecipeCalledIngredient> builder)
    {
    }
}
