using FluentValidation;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.UpdateCookedRecipeCalledIngredient;

public class UpdateCookedRecipeCalledIngredientCommandValidator : AbstractValidator<UpdateCookedRecipeCalledIngredientCommand>
{
    public UpdateCookedRecipeCalledIngredientCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
