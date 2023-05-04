using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.CalledIngredients.Commands.UpdateCalledIngredient;

public record UpdateCalledIngredientCommand : IRequest
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public float Units { get; init; }
}

public class UpdateCalledIngredientCommandHandler : IRequestHandler<UpdateCalledIngredientCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCalledIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCalledIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CalledIngredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CalledIngredient), request.Id);
        }

        entity.Name = request.Name;
        entity.Units = request.Units;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
