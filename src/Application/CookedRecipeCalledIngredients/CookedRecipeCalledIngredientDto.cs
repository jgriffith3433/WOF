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
    public int CookedRecipeId { get; set; }
    public CalledIngredientDto? CalledIngredient { get; set; }
    public ProductStockDto? ProductStock { get; set; }
    public int? ProductStockId { get; set; }
    public string Name { get; set; }
    public UnitType UnitType { get; set; }
    public float Units { get; set; }

    public IList<CookedRecipe> CookedRecipeRecipes { get; private set; } = new List<CookedRecipe>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CookedRecipeCalledIngredient, CookedRecipeCalledIngredientDto>()
            .ForMember(d => d.UnitType, opt => opt.MapFrom(s => (int)s.UnitType));

        profile.CreateMap<CookedRecipeCalledIngredient, CookedRecipeCalledIngredientDto>()
            .ForMember(d => d.CookedRecipeId, opt => opt.MapFrom(s => s.CookedRecipe.Id));

        profile.CreateMap<CookedRecipeCalledIngredient, CookedRecipeCalledIngredientDto>()
            .ForMember(d => d.ProductStockId, opt => opt.MapFrom(mapExpression: s => (int?)(s.ProductStock != null ? s.ProductStock.Id : null)));
    }
}
