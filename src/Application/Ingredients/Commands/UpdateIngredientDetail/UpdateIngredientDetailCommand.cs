using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Enums;
using MediatR;

namespace WOF.Application.Ingredients.Commands.UpdateIngredientDetail;

public record UpdateIngredientDetailCommand : IRequest
{
    public int Id { get; init; }

    public UnitType UnitType { get; init; }

    public string? Name { get; init; }

    public int? WalmartId { get; init; }
}

public class UpdateIngredientDetailCommandHandler : IRequestHandler<UpdateIngredientDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateIngredientDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateIngredientDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Ingredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Ingredient), request.Id);
        }

        entity.Name = request.Name;
        entity.WalmartId = request.WalmartId;
        entity.UnitType = request.UnitType;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
