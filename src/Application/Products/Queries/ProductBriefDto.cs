using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.Products.Queries;

public class ProductBriefDto : IMapFrom<Product>
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? WalmartId { get; set; }
}
