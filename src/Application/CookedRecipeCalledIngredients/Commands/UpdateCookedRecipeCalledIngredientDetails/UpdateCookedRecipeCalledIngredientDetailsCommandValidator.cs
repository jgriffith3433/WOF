using FluentValidation;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.UpdateCookedRecipeCalledIngredientDetails;

public class UpdateCookedRecipeCalledIngredientDetailsCommandValidator : AbstractValidator<UpdateCookedRecipeCalledIngredientDetailsCommand>
{
    public UpdateCookedRecipeCalledIngredientDetailsCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
