using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.ProductStocks.Commands.CreateProductStock;

public record CreateProductStockCommand : IRequest<int>
{
    public string Name { get; init; }
}

public class CreateProductStockCommandHandler : IRequestHandler<CreateProductStockCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductStockCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductStockCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductStock
        {
            Name = request.Name
        };

        _context.ProductStocks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
