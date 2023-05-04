using FluentValidation;

namespace WOF.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateCompletedOrderProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
