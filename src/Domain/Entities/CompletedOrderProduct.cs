namespace WOF.Domain.Entities;

public class CompletedOrderProduct : BaseAuditableEntity
{
    public int CompletedOrderId { get; set; }
    public CompletedOrder CompletedOrder { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
