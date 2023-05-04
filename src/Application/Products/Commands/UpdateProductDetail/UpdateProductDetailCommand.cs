using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Enums;
using MediatR;
using WOF.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Products.Commands.UpdateProductDetail;

public record UpdateProductDetailCommand : IRequest
{
    public int Id { get; init; }

    public SizeType SizeType { get; init; }

    public string? Name { get; init; }

    public int? WalmartId { get; init; }

    public bool Verified { get; init; }
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
        entity.SizeType = request.SizeType;

        //TODO: Come back to manual verification of products
        //entity.Verified = request.Verified;

        if (request.WalmartId != null && entity.WalmartId != request.WalmartId)
        {
            var foundProduct = _context.Products.FirstOrDefault(p => p.WalmartId == request.WalmartId);
            if (foundProduct != null)
            {
                //found a product in database that has walmart id. Merging.
                foreach (var completedOrder in entity.CompletedOrders)
                {
                    foundProduct.CompletedOrders.Add(completedOrder);
                }
                entity.CompletedOrders.Clear();

                var productStocks = _context.ProductStocks.Where(ps => ps.Product == entity);
                foreach (var productStock in productStocks)
                {
                    productStock.Product = foundProduct;
                }
                _context.Products.Remove(entity);
            }
            else
            {
                //no product in our database with that walmart id. get details from walmart
                entity.WalmartId = request.WalmartId;
                entity.AddDomainEvent(new ProductDetailWalmartIdUpdatedEvent(entity));
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
