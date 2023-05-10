using FluentValidation;

namespace WOF.Application.CookedRecipeCalledIngredients.Queries.GetCookedRecipeCalledIngredients;

public class SearchProductStockNameQueryValidator : AbstractValidator<SearchProductStockNameQuery>
{
    public SearchProductStockNameQueryValidator()
    {
        RuleFor(x => x.Search)
            .NotEmpty().WithMessage("Search is required.");
    }
}
