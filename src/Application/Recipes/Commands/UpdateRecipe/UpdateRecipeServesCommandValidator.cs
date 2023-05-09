using WOF.Application.Common.Interfaces;
using FluentValidation;

namespace WOF.Application.Recipes.Commands.UpdateRecipes;

public class UpdateRecipeServesCommandValidator : AbstractValidator<UpdateRecipeServesCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecipeServesCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Serves)
            .NotEmpty().WithMessage("Serves is required.");
    }
}
