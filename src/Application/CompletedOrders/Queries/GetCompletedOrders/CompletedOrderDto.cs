using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrderDto : IMapFrom<CompletedOrder>
{
    public CompletedOrderDto()
    {
        CompletedOrderProducts = new List<CompletedOrderProductDto>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string? UserImport { get; set; }

    public IList<CompletedOrderProductDto> CompletedOrderProducts { get; set; }
}
