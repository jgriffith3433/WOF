namespace WOF.Application.Stocks.Queries.GetStock;

public class GetStockVm
{
    public IList<StockDto> Products { get; set; } = new List<StockDto>();
}
