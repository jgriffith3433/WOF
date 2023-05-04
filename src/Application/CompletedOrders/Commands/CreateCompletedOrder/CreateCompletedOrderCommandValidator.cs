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

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(v => v.UserImport)
            .MaximumLength(40000).WithMessage("UserImport must not exceed 40000 characters.");
    }
}
