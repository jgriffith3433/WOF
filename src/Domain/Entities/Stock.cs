namespace WOF.Domain.Entities;

public class Stock : BaseAuditableEntity
{
    public string Name { get; set; }
    public int Units { get; set; }
    public UnitType UnitType { get; set; }
    public Product Product { get; set; }
}
