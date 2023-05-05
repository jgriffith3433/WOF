using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Commands;

public class CompletedOrderProductBriefDto : IMapFrom<CompletedOrderProduct>
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public long? WalmartId { get; set; }

    public float Size { get; set; }

    public float Price { get; set; }

    public bool Verified { get; set; }
}
