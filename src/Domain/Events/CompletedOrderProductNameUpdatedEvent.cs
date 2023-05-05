namespace WOF.Domain.Events;

public class CompletedOrderProductNameUpdatedEvent : BaseEvent
{
    public CompletedOrderProductNameUpdatedEvent(CompletedOrderProduct completedOrderProduct)
    {
        CompletedOrderProduct = completedOrderProduct;
    }

    public CompletedOrderProduct CompletedOrderProduct { get; }
}
