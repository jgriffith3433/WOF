namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrdersVm
{
    public IList<CompletedOrderDto> CompletedOrders { get; set; } = new List<CompletedOrderDto>();
}
