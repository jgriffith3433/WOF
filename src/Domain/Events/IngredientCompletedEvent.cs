namespace WOF.Domain.Events;

public class IngredientCompletedEvent : BaseEvent
{
    public IngredientCompletedEvent(Ingredient ingredient)
    {
        Ingredient = ingredient;
    }

    public Ingredient Ingredient { get; }
}
