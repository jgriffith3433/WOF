using WOF.Application.Products;

namespace WOF.Application.Recipes.Queries.GetRecipes;

public class RecipesVm
{
    public IList<SizeTypeDto> SizeTypes { get; set; } = new List<SizeTypeDto>();
    public IList<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
}
