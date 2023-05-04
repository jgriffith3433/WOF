namespace WOF.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; }
    public int? WalmartId { get; set; }
    public string? WalmartLink { get; set; }
    public string? WalmartSize { get; set; }
    public string? WalmartItemResponse { get; set; }
    public string? WalmartSearchResponse { get; set; }
    public string? Error { get; set; }
    public float Size { get; set; }
    public float Price { get; set; }
    public bool Verified { get; set; }
    public SizeType SizeType { get; set; }
    public IList<CompletedOrderProduct> CompletedOrders { get; private set; } = new List<CompletedOrderProduct>();
}
