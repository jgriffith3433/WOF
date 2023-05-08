using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.ProductStocks.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.ProductStocks.Commands.UpdateProductStock;

public record UpdateProductStockDetailsCommand : IRequest<ProductStockDto>
{
    public int Id { get; init; }

    public int ProductId { get; init; }

    public float Units { get; init; }
}

public class UpdateProductStockDetailsCommandHandler : IRequestHandler<UpdateProductStockDetailsCommand, ProductStockDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProductStockDetailsCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductStockDto> Handle(UpdateProductStockDetailsCommand request, CancellationToken cancellationToken)
    {
        var productStockEntity = await _context.ProductStocks.Include(p => p.Product).FirstOrDefaultAsync(ps => ps.Id == request.Id, cancellationToken);

        if (productStockEntity == null)
        {
            throw new NotFoundException(nameof(ProductStock), request.Id);
        }

        if (productStockEntity.Product == null || request.ProductId != productStockEntity.Product.Id)
        {
            //First search for product stocks that are already linked to the product
            var alreadyLinkedProductStock = _context.ProductStocks.FirstOrDefault(p => p.Product != null && p.Product.Id == request.ProductId);
            if (alreadyLinkedProductStock != null)
            {
                //we found an existing product stock, merge
                var calledIngredients = _context.CalledIngredients.Where(ci => ci.ProductStock != null && ci.ProductStock.Id == productStockEntity.Id);
                foreach(var calledIngredient in calledIngredients)
                {
                    calledIngredient.ProductStock = alreadyLinkedProductStock;
                }
                _context.ProductStocks.Remove(productStockEntity);
                productStockEntity = alreadyLinkedProductStock;
            }
            else
            {
                //no product stock found linked to product, link this one
                var productEntity = await _context.Products
                    .FindAsync(new object[] { request.ProductId }, cancellationToken);
                if (productStockEntity == null)
                {
                    throw new NotFoundException(nameof(Product), request.ProductId);
                }
                productStockEntity.Product = productEntity;
            }
        }

        productStockEntity.Units = request.Units;

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductStockDto>(productStockEntity);
    }
}
