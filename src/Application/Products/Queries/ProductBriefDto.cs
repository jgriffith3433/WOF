using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.Products.Queries;

public class ProductBriefDto : IMapFrom<Product>
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? WalmartId { get; set; }

    public float Size { get; set; }

    public float Price { get; set; }

    public bool Verified { get; set; }
}
