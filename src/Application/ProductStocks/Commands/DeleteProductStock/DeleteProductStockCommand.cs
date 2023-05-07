using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.ProductStocks.Commands.DeleteProductStock;

public record DeleteProductStockCommand(int Id) : IRequest;

public class DeleteProductStockCommandHandler : IRequestHandler<DeleteProductStockCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductStockCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductStockCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductStocks
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductStock), request.Id);
        }

        _context.ProductStocks.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
