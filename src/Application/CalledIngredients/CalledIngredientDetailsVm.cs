using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.CalledIngredients;

public class CalledIngredientDetailsVm : IMapFrom<CalledIngredient>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductStock ProductStock { get; set; }
    public float? Units { get; set; }
    public int SizeType { get; set; }
    public int ProductStockId { get; set; }
    //public SizeType SizeType { get; set; }

    public IList<ProductStock> ProductStockSearchItems { get; set; } = new List<ProductStock>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CalledIngredient, CalledIngredientDetailsVm>()
            .ForMember(d => d.SizeType, opt => opt.MapFrom(s => (int)s.SizeType));

        profile.CreateMap<CalledIngredient, CalledIngredientDetailsVm>()
            .ForMember(d => d.ProductStockId, opt => opt.MapFrom(mapExpression: s => s.ProductStock != null ? s.ProductStock.Id : -1));
    }
}
