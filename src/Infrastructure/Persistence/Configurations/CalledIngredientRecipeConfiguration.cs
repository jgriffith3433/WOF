using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class CalledIngredientRecipeConfiguration : IEntityTypeConfiguration<CalledIngredientRecipe>
{
    public void Configure(EntityTypeBuilder<CalledIngredientRecipe> builder)
    {
        builder.Property(t => t.CalledIngredientId)
            .IsRequired();

        builder.Property(t => t.RecipeId)
            .IsRequired();
    }
}
