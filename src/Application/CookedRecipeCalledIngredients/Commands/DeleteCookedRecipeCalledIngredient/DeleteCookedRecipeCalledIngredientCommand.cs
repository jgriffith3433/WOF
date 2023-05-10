using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.DeleteCookedRecipeCalledIngredient;

public record DeleteCookedRecipeCalledIngredientCommand(int Id) : IRequest;

public class DeleteCookedRecipeCalledIngredientCommandHandler : IRequestHandler<DeleteCookedRecipeCalledIngredientCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCookedRecipeCalledIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCookedRecipeCalledIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CookedRecipeCalledIngredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CookedRecipe), request.Id);
        }

        _context.CookedRecipeCalledIngredients.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
