using WOF.Application.Recipes.Queries.GetRecipes;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

public class GetCalledIngredientsVm
{
    public IList<CalledIngredientDto> CalledIngredients { get; set; } = new List<CalledIngredientDto>();
}
