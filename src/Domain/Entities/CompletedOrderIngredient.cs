namespace WOF.Domain.Entities;

public class CompletedOrderIngredient : BaseAuditableEntity
{
    public int CompletedOrderId { get; set; }
    public CompletedOrder CompletedOrder { get; set; }
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }
}
