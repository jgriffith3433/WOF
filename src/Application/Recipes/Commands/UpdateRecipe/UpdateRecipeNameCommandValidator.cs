using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Recipes.Commands.UpdateRecipes;

public class UpdateRecipeNameCommandValidator : AbstractValidator<UpdateRecipeNameCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecipeNameCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Recipe name must not exceed 200 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Recipes
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
