using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WOF.Infrastructure.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.UserImport)
            .IsRequired();

        builder.Property(t => t.Link)
            .HasMaxLength(4000);

        //builder.HasMany(left => left.CalledIngredients).WithMany(right => right.Recipe).UsingEntity("CalledIngredientRecipe", typeof(Dictionary<string, object>),
        //    right => right.HasOne(typeof(CalledIngredient)).WithMany().HasForeignKey("CalledIngredientId"),
        //    left => left.HasOne(typeof(Recipe)).WithMany().HasForeignKey("RecipeId"),
        //    join => join.ToTable("CalledIngredientRecipes")
        //);
    }
}
