using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Enums;
using MediatR;

namespace WOF.Application.CalledIngredients.Commands.UpdateCalledIngredientDetail;

public record UpdateCalledIngredientDetailsCommand : IRequest
{
    public int Id { get; init; }

    public SizeType SizeType { get; init; }

    public int? ProductStockId { get; init; }

    public string? Name { get; init; }

    public float Units { get; init; }
}

public class UpdateCalledIngredientDetailCommandHandler : IRequestHandler<UpdateCalledIngredientDetailsCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCalledIngredientDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCalledIngredientDetailsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CalledIngredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CalledIngredient), request.Id);
        }

        entity.Name = request.Name;
        entity.Units = request.Units;
        entity.SizeType = request.SizeType;
        if (request.ProductStockId != null)
        {
            entity.ProductStock = _context.ProductStocks.FirstOrDefault(ps => ps.Id == request.ProductStockId);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
