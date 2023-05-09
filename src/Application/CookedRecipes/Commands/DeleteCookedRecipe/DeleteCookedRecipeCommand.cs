using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CookedRecipes.Commands.DeleteCookedRecipes;

public record DeleteCookedRecipeCommand(int Id) : IRequest;

public class DeleteCookedRecipeCommandHandler : IRequestHandler<DeleteCookedRecipeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCookedRecipeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCookedRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CookedRecipes
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CookedRecipe), request.Id);
        }

        _context.CookedRecipes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
