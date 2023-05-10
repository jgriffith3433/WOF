using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.UpdateCookedRecipeCalledIngredient;

public record UpdateCookedRecipeCalledIngredientCommand : IRequest
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public float? Units { get; init; }

    public int? ProductStockId { get; init; }
}

public class UpdateCookedRecipeCalledIngredientCommandHandler : IRequestHandler<UpdateCookedRecipeCalledIngredientCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCookedRecipeCalledIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCookedRecipeCalledIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CookedRecipeCalledIngredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CookedRecipeCalledIngredient), request.Id);
        }

        entity.Name = request.Name;
        entity.Units = request.Units.Value;

        if (request.ProductStockId != null)
        {
            var productStock = _context.ProductStocks.FirstOrDefault(ps => ps.Id == request.ProductStockId);

            if (productStock == null)
            {
                throw new NotFoundException(nameof(ProductStock), request.Id);
            }
            entity.ProductStock = productStock;
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
