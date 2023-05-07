using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Application.Products.Queries;
using WOF.Domain.Entities;

namespace WOF.Application.ProductStocks.Queries;

public class ProductStockDetailsVm : IMapFrom<ProductStock>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public float Units { get; set; }

    public int? ProductId { get; set; }

    public ProductDto Product { get; set; }

    public IList<ProductDto> ProductSearchItems { get; set; } = new List<ProductDto>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductStock, ProductStockDetailsVm>()
            .ForMember(d => d.ProductId, opt => opt.MapFrom(s => (int)s.Product.Id));
    }
}
