using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrderDto : IMapFrom<CompletedOrder>
{
    public CompletedOrderDto()
    {
        Products = new List<ProductDto>();
    }

    public int Id { get; set; }

    public string? UserImport { get; set; }

    public IList<ProductDto> Products { get; set; }
}
