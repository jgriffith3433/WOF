using FluentValidation;

namespace WOF.Application.CalledIngredients.Commands.UpdateCalledIngredient;

public class UpdateCalledIngredientCommandValidator : AbstractValidator<UpdateCalledIngredientCommand>
{
    public UpdateCalledIngredientCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
