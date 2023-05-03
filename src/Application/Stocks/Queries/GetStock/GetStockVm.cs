namespace WOF.Application.Stocks.Queries.GetStock;

public class GetStockVm
{
    public IList<StockDto> Ingredients { get; set; } = new List<StockDto>();
}
