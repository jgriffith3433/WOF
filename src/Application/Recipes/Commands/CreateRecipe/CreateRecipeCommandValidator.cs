using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Recipes.Commands.CreateRecipes;

public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateRecipeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserImport)
            .NotEmpty().WithMessage("UserImport is required.")
            .MaximumLength(40000).WithMessage("UserImport must not exceed 40000 characters.")
            .MustAsync(BeUniqueUserImport).WithMessage("The specified UserImport already exists.");
    }

    public async Task<bool> BeUniqueUserImport(string userImport, CancellationToken cancellationToken)
    {
        return await _context.Recipes
            .AllAsync(l => l.UserImport != userImport, cancellationToken);
    }
}
