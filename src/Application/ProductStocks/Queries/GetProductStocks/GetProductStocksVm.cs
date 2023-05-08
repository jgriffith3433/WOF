using WOF.Application.Products;

namespace WOF.Application.ProductStocks.Queries.GetProductStocks;

public class GetProductStocksVm
{
    public IList<SizeTypeDto> SizeTypes { get; set; } = new List<SizeTypeDto>();
    public IList<ProductStockDto> ProductStocks { get; set; } = new List<ProductStockDto>();
}
