using AutoMapper;
using WOF.Application.CalledIngredients;
using WOF.Application.Common.Mappings;
using WOF.Application.Products.Queries;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.ProductStocks.Queries;

public class ProductStockDto : IMapFrom<ProductStock>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public float Units { get; set; }

    public int? ProductId { get; set; }

    public ProductDto Product { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductStock, ProductStockDto>()
            .ForMember(d => d.ProductId, opt => opt.MapFrom(s => (int)s.Product.Id));
    }
}
