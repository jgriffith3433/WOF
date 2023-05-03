using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.Products.Queries.GetProducts;

public class GetProductsVm
{
    public IList<ProductDto> Products { get; set; } = new List<ProductDto>();
}
