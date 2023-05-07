using FluentValidation;

namespace WOF.Application.ProductStocks.Commands.CreateProductStock;

public class CreateProductStockCommandValidator : AbstractValidator<CreateProductStockCommand>
{
    public CreateProductStockCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
