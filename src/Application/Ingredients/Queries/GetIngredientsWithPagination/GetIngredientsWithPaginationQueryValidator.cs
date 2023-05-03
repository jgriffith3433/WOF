using FluentValidation;

namespace WOF.Application.Ingredients.Queries.GetIngredientsWithPagination;

public class GetIngredientsWithPaginationQueryValidator : AbstractValidator<GetIngredientsWithPaginationQuery>
{
    public GetIngredientsWithPaginationQueryValidator()
    {
        //TODO: look into this
        //RuleFor(x => x.ListId)
        //    .NotEmpty().WithMessage("ListId is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
