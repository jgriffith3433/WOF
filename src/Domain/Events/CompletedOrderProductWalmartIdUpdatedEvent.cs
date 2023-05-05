namespace WOF.Domain.Events;

public class CompletedOrderProductWalmartIdUpdatedEvent : BaseEvent
{
    public CompletedOrderProductWalmartIdUpdatedEvent(CompletedOrderProduct completedOrderProduct)
    {
        CompletedOrderProduct = completedOrderProduct;
    }

    public CompletedOrderProduct CompletedOrderProduct { get; }
}
