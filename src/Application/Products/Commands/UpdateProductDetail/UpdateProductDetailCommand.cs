using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Enums;
using MediatR;

namespace WOF.Application.Products.Commands.UpdateProductDetail;

public record UpdateProductDetailCommand : IRequest
{
    public int Id { get; init; }

    public UnitType UnitType { get; init; }

    public string? Name { get; init; }

    public int? WalmartId { get; init; }
}

public class UpdateProductDetailCommandHandler : IRequestHandler<UpdateProductDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        entity.Name = request.Name;
        entity.WalmartId = request.WalmartId;
        entity.UnitType = request.UnitType;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
