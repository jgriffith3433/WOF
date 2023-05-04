namespace WOF.Domain.Entities;

public class CompletedOrder : BaseAuditableEntity
{
    public string Name { get; set; }
    public string? UserImport { get; set; }

    public IList<CompletedOrderProduct> Products { get; private set; } = new List<CompletedOrderProduct>();
}
