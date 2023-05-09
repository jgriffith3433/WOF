using AutoMapper;
using WOF.Application.CalledIngredients;
using WOF.Application.Common.Mappings;
using WOF.Application.CookedRecipes.Queries.GetCookedRecipes;
using WOF.Application.ProductStocks.Queries;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.CookedRecipeCalledIngredients;

public class CookedRecipeCalledIngredientDetailsVm : IMapFrom<CookedRecipeCalledIngredient>
{
    public int Id { get; set; }
    public CookedRecipeDto CookedRecipe { get; set; }
    public CalledIngredientDto? CalledIngredient { get; set; }
    public ProductStockDto ProductStock { get; set; }
    public SizeType SizeType { get; set; }
    public float Units { get; set; }

    public IList<ProductStock> ProductStockSearchItems { get; set; } = new List<ProductStock>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CookedRecipeCalledIngredient, CookedRecipeCalledIngredientDetailsVm>()
            .ForMember(d => d.SizeType, opt => opt.MapFrom(s => (int)s.SizeType));
    }
}
