namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrdersVm
{
    public IList<SizeTypeDto> SizeTypes { get; set; } = new List<SizeTypeDto>();
    public IList<CompletedOrderDto> CompletedOrders { get; set; } = new List<CompletedOrderDto>();
}
