using WOF.Application.CookedRecipeCalledIngredients;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;
using AutoMapper;
using WOF.Application.Recipes.Queries.GetRecipes;

namespace WOF.Application.CookedRecipes.Queries.GetCookedRecipes;

public class CookedRecipeDto : IMapFrom<CookedRecipe>
{
    public CookedRecipeDto()
    {
        CookedRecipeCalledIngredients = new List<CookedRecipeCalledIngredientDetailsVm>();
    }

    public int Id { get; set; }

    public RecipeDto Recipe { get; set; }

    public int RecipeId { get; set; }
    
    public IList<CookedRecipeCalledIngredientDetailsVm> CookedRecipeCalledIngredients { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CookedRecipe, CookedRecipeDto>()
            .ForMember(d => d.RecipeId, opt => opt.MapFrom(mapExpression: s => s.Recipe != null ? s.Recipe.Id : -1));
    }
}
