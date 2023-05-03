namespace WOF.Domain.Entities;

public class CompletedOrder : BaseAuditableEntity
{
    public string UserImport { get; set; }

    public IList<Product> Products { get; private set; } = new List<Product>();
}
