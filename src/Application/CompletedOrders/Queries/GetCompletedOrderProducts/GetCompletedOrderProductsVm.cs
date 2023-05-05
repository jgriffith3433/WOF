using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrderProducts;

public class GetCompletedOrderProductsVm
{
    public IList<CompletedOrderProductDto> Products { get; set; } = new List<CompletedOrderProductDto>();
}
