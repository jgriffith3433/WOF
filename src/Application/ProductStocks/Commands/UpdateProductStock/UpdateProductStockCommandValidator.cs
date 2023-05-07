using WOF.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.ProductStocks.Commands.UpdateProductStock;

public class UpdateProductStockCommandValidator : AbstractValidator<UpdateProductStockCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductStockCommandValidator(IApplicationDbContext context)
    {
        _context = context;
    }
}
