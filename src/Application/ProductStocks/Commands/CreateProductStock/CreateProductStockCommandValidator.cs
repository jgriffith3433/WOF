using FluentValidation;

namespace WOF.Application.CalledIngredients.Commands.CreateCalledIngredient;

public class CreateProductStockCommandValidator : AbstractValidator<CreateCalledIngredientCommand>
{
    public CreateProductStockCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
