namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrdersVm
{
    public IList<UnitTypeDto> UnitTypes { get; set; } = new List<UnitTypeDto>();
    public IList<CompletedOrderDto> CompletedOrders { get; set; } = new List<CompletedOrderDto>();
}
