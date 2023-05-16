using WOF.Application.Products;

namespace WOF.Application.CookedRecipes.Queries.GetCookedRecipes;

public class CookedRecipesVm
{
    public IList<UnitTypeDto> UnitTypes { get; set; } = new List<UnitTypeDto>();
    public IList<CookedRecipeDto> CookedRecipes { get; set; } = new List<CookedRecipeDto>();
    public IList<RecipesOptionVm> RecipesOptions { get; set; } = new List<RecipesOptionVm>();
}
