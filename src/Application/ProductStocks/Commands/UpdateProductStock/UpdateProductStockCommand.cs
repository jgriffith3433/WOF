using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.ProductStocks.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.ProductStocks.Commands.UpdateProductStock;

public record UpdateProductStockCommand : IRequest<ProductStockDto>
{
    public int Id { get; init; }

    public int Units { get; init; }
}

public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, ProductStockDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProductStockCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductStockDto> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        var productStockEntity = await _context.ProductStocks.Include(p => p.Product).FirstOrDefaultAsync(ps => ps.Id == request.Id, cancellationToken);

        if (productStockEntity == null)
        {
            throw new NotFoundException(nameof(ProductStock), request.Id);
        }

        productStockEntity.Units = request.Units;

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductStockDto>(productStockEntity);
    }
}
