namespace WOF.Domain.Entities;

public class CookedRecipe : BaseAuditableEntity
{
    public Recipe Recipe { get; set; }

    public IList<CookedRecipeCalledIngredient> CookedRecipeCalledIngredients { get; private set; } = new List<CookedRecipeCalledIngredient>();
}
