namespace WOF.Application.Ingredients.Queries.GetIngredients;

public class GetIngredientsVm
{
    public IList<IngredientBriefDto> Ingredients { get; set; } = new List<IngredientBriefDto>();
}
