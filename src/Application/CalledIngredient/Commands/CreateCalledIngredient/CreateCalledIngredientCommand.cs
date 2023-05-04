using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;

namespace WOF.Application.CalledIngredients.Commands.CreateCalledIngredient;

public record CreateCalledIngredientCommand : IRequest<int>
{
    public string? Name { get; init; }
    public float Units { get; init; }
}

public class CreateCalledIngredientCommandHandler : IRequestHandler<CreateCalledIngredientCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCalledIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCalledIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = new CalledIngredient
        {
            Name = request.Name,
            Units = request.Units
        };

        entity.AddDomainEvent(new CalledIngredientCreatedEvent(entity));

        _context.CalledIngredients.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
