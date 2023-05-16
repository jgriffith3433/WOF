using WOF.Application.Products;

namespace WOF.Application.ProductStocks.Queries.GetProductStocks;

public class GetProductStocksVm
{
    public IList<UnitTypeDto> UnitTypes { get; set; } = new List<UnitTypeDto>();
    public IList<ProductStockDto> ProductStocks { get; set; } = new List<ProductStockDto>();
}
