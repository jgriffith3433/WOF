namespace WOF.Domain.Events;

public class CalledIngredientCompletedEvent : BaseEvent
{
    public CalledIngredientCompletedEvent(CalledIngredient calledIngredient)
    {
        CalledIngredient = calledIngredient;
    }

    public CalledIngredient CalledIngredient { get; }
}
