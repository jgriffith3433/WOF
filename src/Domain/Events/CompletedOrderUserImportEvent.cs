namespace WOF.Domain.Events;

public class CompletedOrderUserImportEvent : BaseEvent
{
    public CompletedOrderUserImportEvent(CompletedOrder completedOrder)
    {
        CompletedOrder = completedOrder;
    }

    public CompletedOrder CompletedOrder { get; }
}
