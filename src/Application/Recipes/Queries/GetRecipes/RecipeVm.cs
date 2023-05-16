using WOF.Application.Products;

namespace WOF.Application.Recipes.Queries.GetRecipes;

public class RecipesVm
{
    public IList<UnitTypeDto> UnitTypes { get; set; } = new List<UnitTypeDto>();
    public IList<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
}
