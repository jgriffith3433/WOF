namespace WOF.Domain.Entities;

public class CalledIngredientRecipe : BaseAuditableEntity
{
    public int CalledIngredientId { get; set; }
    public CalledIngredient CalledIngredient { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}
