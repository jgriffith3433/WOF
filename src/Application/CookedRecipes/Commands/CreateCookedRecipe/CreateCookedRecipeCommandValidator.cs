using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
namespace WOF.Application.CookedRecipes.Commands.CreateCookedRecipes;

public class CreateCookedRecipeCommandValidator : AbstractValidator<CreateCookedRecipeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCookedRecipeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.RecipeId)
            .NotEmpty().WithMessage("Recipe is required");
    }
}
