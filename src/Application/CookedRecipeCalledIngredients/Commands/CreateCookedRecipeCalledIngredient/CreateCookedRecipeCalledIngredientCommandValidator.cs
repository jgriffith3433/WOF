using FluentValidation;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.CreateCookedRecipeCalledIngredient;

public class CreateCookedRecipeCalledIngredientCommandValidator : AbstractValidator<CreateCookedRecipeCalledIngredientCommand>
{
    public CreateCookedRecipeCalledIngredientCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
