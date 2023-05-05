using FluentValidation;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrderProducts;

public class SearchCompletedOrderProductNameQueryValidator : AbstractValidator<SearchCompletedOrderProductNameQuery>
{
    public SearchCompletedOrderProductNameQueryValidator()
    {
        RuleFor(x => x.Search)
            .NotEmpty().WithMessage("Name is required.");
    }
}
