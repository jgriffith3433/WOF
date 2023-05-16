namespace WOF.Domain.Entities;

public class CookedRecipeCalledIngredient : BaseAuditableEntity
{
    public CookedRecipe CookedRecipe { get; set; }
    public CalledIngredient? CalledIngredient { get; set; }
    public ProductStock? ProductStock { get; set; }
    public string Name { get; set; }
    public UnitType UnitType { get; set; }
    public float Units { get; set; }
}
