﻿using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Recipes.Commands.UpdateRecipes;

public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecipeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserImport)
            .NotEmpty().WithMessage("UserImport is required.")
            .MaximumLength(40000).WithMessage("UserImport must not exceed 40000 characters.")
            .MustAsync(BeUniqueUserImport).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueUserImport(UpdateRecipeCommand model, string userImport, CancellationToken cancellationToken)
    {
        return await _context.Recipes
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.UserImport != userImport, cancellationToken);
    }
}
