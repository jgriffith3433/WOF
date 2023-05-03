namespace WOF.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; }
    public int? WalmartId { get; set; }
    public UnitType UnitType { get; set; }
    public IList<CompletedOrder> CompletedOrders { get; private set; } = new List<CompletedOrder>();
}
