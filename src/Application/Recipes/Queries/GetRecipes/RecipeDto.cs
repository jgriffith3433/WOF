using WOF.Application.CalledIngredients;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.Recipes.Queries.GetRecipes;

public class RecipeDto : IMapFrom<Recipe>
{
    public RecipeDto()
    {
        CalledIngredients = new List<CalledIngredientDetailsVm>();
    }

    public int Id { get; set; }
    
    public string Name { get; set; }

    public string? UserImport { get; set; }

    public IList<CalledIngredientDetailsVm> CalledIngredients { get; set; }
}
