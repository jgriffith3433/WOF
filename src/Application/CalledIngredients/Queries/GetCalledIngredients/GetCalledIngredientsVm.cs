using WOF.Application.CalledIngredients;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

public class GetCalledIngredientsVm
{
    public IList<CalledIngredientDto> CalledIngredients { get; set; } = new List<CalledIngredientDto>();
}
