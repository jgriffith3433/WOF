namespace WOF.Domain.Events;

public class CompletedOrderProductCreatedEvent : BaseEvent
{
    public CompletedOrderProductCreatedEvent(CompletedOrderProduct completedOrderProduct)
    {
        CompletedOrderProduct = completedOrderProduct;
    }

    public CompletedOrderProduct CompletedOrderProduct { get; }
}
