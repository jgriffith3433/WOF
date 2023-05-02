namespace WOF.Domain.Entities;

public class CalledIngredient : BaseAuditableEntity
{
    public string Name { get; set; }

    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; } = null!;

    public IList<Recipe> Recipes { get; private set; } = new List<Recipe>();
}
