using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.CalledIngredients;

public class CalledIngredientDto : IMapFrom<CalledIngredient>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductStock ProductStock { get; set; }
    public float Units { get; set; }
    public int UnitType { get; set; }
    //public UnitType UnitType { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CalledIngredient, CalledIngredientDto>()
            .ForMember(d => d.UnitType, opt => opt.MapFrom(s => (int)s.UnitType));
    }
}
