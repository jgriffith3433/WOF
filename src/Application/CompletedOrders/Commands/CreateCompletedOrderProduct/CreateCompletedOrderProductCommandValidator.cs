using FluentValidation;

namespace WOF.Application.CompletedOrders.Commands.CreateCompletedOrderProduct;

public class CreateCompletedOrderProductCommandValidator : AbstractValidator<CreateCompletedOrderProductCommand>
{
    public CreateCompletedOrderProductCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
