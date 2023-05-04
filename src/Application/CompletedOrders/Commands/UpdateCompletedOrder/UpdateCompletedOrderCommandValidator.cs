using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CompletedOrders.Commands.UpdateCompletedOrders;

public class UpdateCompletedOrderCommandValidator : AbstractValidator<UpdateCompletedOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompletedOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserImport)
            .NotEmpty().WithMessage("UserImport is required.")
            .MaximumLength(40000).WithMessage("UserImport must not exceed 40000 characters.");
    }
}
