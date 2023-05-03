using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.ProductStocks.Queries;

public class ProductStockDto : IMapFrom<ProductStock>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public float Units { get; set; }
}
