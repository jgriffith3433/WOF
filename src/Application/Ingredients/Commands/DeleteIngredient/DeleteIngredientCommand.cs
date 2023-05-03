using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;

namespace WOF.Application.Ingredients.Commands.DeleteIngredient;

public record DeleteIngredientCommand(int Id) : IRequest;

public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Ingredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        _context.Ingredients.Remove(entity);

        entity.AddDomainEvent(new IngredientDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
