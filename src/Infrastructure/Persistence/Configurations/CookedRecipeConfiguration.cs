using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CookedRecipeConfiguration : IEntityTypeConfiguration<CookedRecipe>
{
    public void Configure(EntityTypeBuilder<CookedRecipe> builder)
    {
        builder.HasMany(cr => cr.CookedRecipeCalledIngredients)
            .WithOne(crci => crci.CookedRecipe);
    }
}
