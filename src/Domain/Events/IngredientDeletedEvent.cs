namespace WOF.Domain.Events;

public class IngredientDeletedEvent : BaseEvent
{
    public IngredientDeletedEvent(Ingredient ingredient)
    {
        Ingredient = ingredient;
    }

    public Ingredient Ingredient { get; }
}
