namespace WOF.Domain.Entities;

public class ProductStock : BaseAuditableEntity
{
    public string Name { get; set; }
    public float Units { get; set; }
    public Product Product { get; set; }
}
