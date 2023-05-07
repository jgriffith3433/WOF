using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Common.Interfaces;

namespace WOF.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.WalmartId)
            .MustAsync(BeUniqueWalmartId).WithMessage("The specified product already exists.");
    }

    public async Task<bool> BeUniqueWalmartId(long? walmartId, CancellationToken cancellationToken)
    {
        if (walmartId == null)
        {
            return true;
        }
        return await _context.Products
            .AllAsync(l => l.WalmartId != walmartId, cancellationToken);
    }
}
