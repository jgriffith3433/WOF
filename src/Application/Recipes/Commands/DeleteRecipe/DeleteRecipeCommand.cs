using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Recipes.Commands.DeleteRecipes;

public record DeleteRecipeCommand(int Id) : IRequest;

public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteRecipeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Recipes
            .Where(l => l.Id == request.Id).Include(r => r.CalledIngredients)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Recipe), request.Id);
        }
        foreach(var calledIngredient in entity.CalledIngredients)
        {
            _context.CalledIngredients.Remove(calledIngredient);
        }
        _context.Recipes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
