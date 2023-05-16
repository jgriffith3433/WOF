namespace WOF.Domain.Entities;

public class CalledIngredient : BaseAuditableEntity
{
    public string Name { get; set; }

    public float? Units { get; set; }

    public bool Verified { get; set; }

    public SizeType SizeType { get; set; }

    public ProductStock? ProductStock { get; set; } = null!;

    public Recipe Recipe { get; set; } = null!;
}
