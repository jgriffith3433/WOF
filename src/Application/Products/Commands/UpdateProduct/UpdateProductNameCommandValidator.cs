using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Common.Interfaces;

namespace WOF.Application.Products.Commands.UpdateProduct;

public class UpdateProductNameCommandValidator : AbstractValidator<UpdateProductNameCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductNameCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
            .NotEmpty().WithMessage("Name is required.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
