namespace WOF.Domain.Events;

public class CalledIngredientDeletedEvent : BaseEvent
{
    public CalledIngredientDeletedEvent(CalledIngredient calledIngredient)
    {
        CalledIngredient = calledIngredient;
    }

    public CalledIngredient CalledIngredient { get; }
}
