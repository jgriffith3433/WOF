namespace WOF.Domain.Entities;

public class ConsumedCookedRecipe : BaseAuditableEntity
{
    public Recipe Recipe { get; set; }

    public float Servings { get; set; }
}
