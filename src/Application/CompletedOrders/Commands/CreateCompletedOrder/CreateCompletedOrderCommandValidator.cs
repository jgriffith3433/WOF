using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CompletedOrders.Commands.CreateCompletedOrders;

public class CreateCompletedOrderCommandValidator : AbstractValidator<CreateCompletedOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCompletedOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserImport)
            .NotEmpty().WithMessage("UserImport is required.")
            .MaximumLength(40000).WithMessage("UserImport must not exceed 40000 characters.")
            .MustAsync(BeUniqueUserImport).WithMessage("The specified UserImport already exists.");
    }

    public async Task<bool> BeUniqueUserImport(string userImport, CancellationToken cancellationToken)
    {
        return await _context.CompletedOrders
            .AllAsync(l => l.UserImport != userImport, cancellationToken);
    }
}
