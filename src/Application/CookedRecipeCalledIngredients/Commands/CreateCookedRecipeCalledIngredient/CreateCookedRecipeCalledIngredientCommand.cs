using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Common.Exceptions;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.CreateCookedRecipeCalledIngredient;

public record CreateCookedRecipeCalledIngredientCommand : IRequest<int>
{
    public string? Name { get; init; }
    public int CookedRecipeId { get; init; }
    public int? ProductStockId { get; init; }
}

public class CreateCookedRecipeCalledIngredientCommandHandler : IRequestHandler<CreateCookedRecipeCalledIngredientCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCookedRecipeCalledIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCookedRecipeCalledIngredientCommand request, CancellationToken cancellationToken)
    {
        var cookedRecipe = _context.CookedRecipes.FirstOrDefault(cr => cr.Id == request.CookedRecipeId);

        if (cookedRecipe == null)
        {
            throw new NotFoundException(nameof(CookedRecipe), "CookedRecipe not found");
        }

        var entity = new CookedRecipeCalledIngredient
        {
            Name = request.Name,
            CookedRecipe = cookedRecipe
        };

        if (request.ProductStockId != null)
        {
            var productStock = _context.ProductStocks.FirstOrDefault(ps => ps.Id == request.ProductStockId);

            if (productStock == null)
            {
                throw new NotFoundException(nameof(ProductStock), "ProductStock not found");
            }

            entity.ProductStock = productStock;
        }

        _context.CookedRecipeCalledIngredients.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
