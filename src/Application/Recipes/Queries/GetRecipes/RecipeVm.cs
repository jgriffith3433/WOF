using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.Recipes.Queries.GetRecipes;

public class RecipesVm
{
    public IList<SizeTypeDto> SizeTypes { get; set; } = new List<SizeTypeDto>();
    public IList<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
}
