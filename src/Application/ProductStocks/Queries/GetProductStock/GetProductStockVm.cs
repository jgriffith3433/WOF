namespace WOF.Application.ProductStocks.Queries.GetProductStock;

public class GetProductStockVm
{
    public IList<ProductStockDto> ProductStocks { get; set; } = new List<ProductStockDto>();
}
