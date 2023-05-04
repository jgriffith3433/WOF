namespace WOF.Domain.Events;

public class CalledIngredientCreatedEvent : BaseEvent
{
    public CalledIngredientCreatedEvent(CalledIngredient calledIngredient)
    {
        CalledIngredient = calledIngredient;
    }

    public CalledIngredient CalledIngredient { get; }
}
