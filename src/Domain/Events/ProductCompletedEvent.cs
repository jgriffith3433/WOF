namespace WOF.Domain.Events;

public class ProductCompletedEvent : BaseEvent
{
    public ProductCompletedEvent(Product calledIngredient)
    {
        CalledIngredient = calledIngredient;
    }

    public Product CalledIngredient { get; }
}
