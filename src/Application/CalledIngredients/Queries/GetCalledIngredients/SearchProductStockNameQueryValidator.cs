using FluentValidation;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

public class SearchProductStockNameQueryValidator : AbstractValidator<SearchProductStockNameQuery>
{
    public SearchProductStockNameQueryValidator()
    {
        RuleFor(x => x.Search)
            .NotEmpty().WithMessage("Name is required.");
    }
}
