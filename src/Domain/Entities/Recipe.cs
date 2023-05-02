namespace WOF.Domain.Entities;

public class Recipe : BaseAuditableEntity
{
    public string Name { get; set; }
    public string UserImport { get; set; }
    public string Link { get; set; }

    public IList<CalledIngredient> CalledIngredients { get; private set; } = new List<CalledIngredient>();
}
