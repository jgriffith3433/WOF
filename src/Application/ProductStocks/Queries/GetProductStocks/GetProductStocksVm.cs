namespace WOF.Application.ProductStocks.Queries.GetProductStocks;

public class GetProductStocksVm
{
    public IList<ProductStockDto> ProductStocks { get; set; } = new List<ProductStockDto>();
}
