using FluentValidation;

namespace WOF.Application.CompletedOrders.Commands.UpdateCompletedOrderProduct;

public class UpdateCompletedOrderProductCommandValidator : AbstractValidator<UpdateCompletedOrderProductCommand>
{
    public UpdateCompletedOrderProductCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
