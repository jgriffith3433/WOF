namespace WOF.Domain.Entities;

public class Ingredient : BaseAuditableEntity
{
    public string Name { get; set; }
    public int? WalmartId { get; set; }

    public IList<CompletedOrder> CompletedOrders { get; private set; } = new List<CompletedOrder>();
}
