namespace WOF.Domain.Events;

public class CompletedOrderProductDeletedEvent : BaseEvent
{
    public CompletedOrderProductDeletedEvent(CompletedOrderProduct completedOrderProduct)
    {
        CompletedOrderProduct = completedOrderProduct;
    }

    public CompletedOrderProduct CompletedOrderProduct { get; }
}
