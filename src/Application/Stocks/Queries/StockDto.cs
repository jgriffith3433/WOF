using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.Stocks.Queries;

public class StockDto : IMapFrom<Stock>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Units { get; set; }
}
