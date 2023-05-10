using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;

namespace WOF.Application.CalledIngredients.Commands.DeleteCalledIngredient;

public record DeleteCalledIngredientCommand(int Id) : IRequest;

public class DeleteCalledIngredientCommandHandler : IRequestHandler<DeleteCalledIngredientCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCalledIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCalledIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CalledIngredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CalledIngredient), request.Id);
        }

        _context.CalledIngredients.Remove(entity);

        entity.AddDomainEvent(new CalledIngredientDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
