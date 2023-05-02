namespace WOF.Domain.Entities;

public class CompletedOrder : BaseAuditableEntity
{
    public string UserImport { get; set; }

    public IList<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
}
