using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;

namespace WOF.Application.Ingredients.Commands.CreateIngredient;

public record CreateIngredientCommand : IRequest<int>
{
    public string? Name { get; init; }
    public int? WalmartId { get; init; }
}

public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = new Ingredient
        {
            Name = request.Name,
            WalmartId = request.WalmartId
        };

        entity.AddDomainEvent(new IngredientCreatedEvent(entity));

        _context.Ingredients.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
