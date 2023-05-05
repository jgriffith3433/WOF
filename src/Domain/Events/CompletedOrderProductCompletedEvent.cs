namespace WOF.Domain.Events;

public class CompletedOrderProductCompletedEvent : BaseEvent
{
    public CompletedOrderProductCompletedEvent(CompletedOrderProduct completedOrderProduct)
    {
        CompletedOrderProduct = completedOrderProduct;
    }

    public CompletedOrderProduct CompletedOrderProduct { get; }
}
