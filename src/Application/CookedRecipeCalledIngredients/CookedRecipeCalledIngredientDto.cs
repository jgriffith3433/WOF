using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Application.CookedRecipes.Queries.GetCookedRecipes;
using WOF.Application.ProductStocks.Queries;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.CalledIngredients;

public class CookedRecipeCalledIngredientDto : IMapFrom<CookedRecipeCalledIngredient>
{
    public int Id { get; set; }
    public CookedRecipeDto CookedRecipe { get; set; }
    public CalledIngredientDto? CalledIngredient { get; set; }
    public ProductStockDto ProductStock { get; set; }
    public SizeType SizeType { get; set; }
    public float Units { get; set; }

    public IList<CookedRecipe> CookedRecipeRecipes { get; private set; } = new List<CookedRecipe>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CookedRecipeCalledIngredient, CookedRecipeCalledIngredientDto>()
            .ForMember(d => d.SizeType, opt => opt.MapFrom(s => (int)s.SizeType));
    }
}
