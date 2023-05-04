using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.Recipes.Queries.GetRecipes;

public class CalledIngredientDto : IMapFrom<CalledIngredient>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductStock ProductStock { get; set; }
    public float Units { get; set; }
    public int SizeType { get; set; }
    //public SizeType SizeType { get; set; }

    public IList<Recipe> Recipes { get; private set; } = new List<Recipe>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CalledIngredient, CalledIngredientDto>()
            .ForMember(d => d.SizeType, opt => opt.MapFrom(s => (int)s.SizeType));
    }
}
