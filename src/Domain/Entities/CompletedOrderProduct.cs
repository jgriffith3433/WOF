namespace WOF.Domain.Entities;

public class CompletedOrderProduct : BaseAuditableEntity
{
    public string Name { get; set; }
    public long? WalmartId { get; set; }
    public string? WalmartItemResponse { get; set; }
    public string? WalmartSearchResponse { get; set; }
    public string? WalmartError { get; set; }
    public CompletedOrder CompletedOrder { get; set; }
    public Product? Product { get; set; }
}
