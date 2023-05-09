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

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

        RuleFor(v => v.Serves)
            .NotEmpty().WithMessage("Serves is required.");

        RuleFor(v => v.UserImport)
            .MaximumLength(40000).WithMessage("User Import must not exceed 40000 characters.")
            .MustAsync(BeUniqueUserImport).WithMessage("The specified user import already exists.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Recipes
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public async Task<bool> BeUniqueUserImport(string userImport, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(userImport))
        {
            return true;
        }
        return await _context.Recipes
            .AllAsync(l => l.UserImport != userImport, cancellationToken);
    }
}
