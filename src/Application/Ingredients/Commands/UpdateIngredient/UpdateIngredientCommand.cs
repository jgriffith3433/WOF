using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.Ingredients.Commands.UpdateIngredient;

public record UpdateIngredientCommand : IRequest
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public int? WalmartId { get; init; }
}

public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateIngredientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Ingredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Ingredient), request.Id);
        }

        entity.Name = request.Name;
        entity.WalmartId = request.WalmartId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
