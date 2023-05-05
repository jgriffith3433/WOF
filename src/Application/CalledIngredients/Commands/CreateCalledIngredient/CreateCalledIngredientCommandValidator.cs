using FluentValidation;

namespace WOF.Application.CalledIngredients.Commands.CreateCalledIngredient;

public class CreateCalledIngredientCommandValidator : AbstractValidator<CreateCalledIngredientCommand>
{
    public CreateCalledIngredientCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
