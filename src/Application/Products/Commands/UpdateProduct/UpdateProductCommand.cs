using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Products.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WOF.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; init; }

    public long? WalmartId { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWalmartApiService _walmartApiService;

    public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper, IWalmartApiService walmartApiService)
    {
        _context = context;
        _mapper = mapper;
        _walmartApiService = walmartApiService;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.Include(p => p.ProductStock).FirstOrDefaultAsync(ps => ps.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        if (entity.WalmartId != request.WalmartId && request.WalmartId != null)
        {
            //Found walmart id by searching and user manually set the product by selecting walmart id

            var existingProductEntity = _context.Products.FirstOrDefault(p => p.WalmartId == request.WalmartId);

            if (existingProductEntity != null)
            {
                throw new Exception("Walmart Id already exists in products");
            }

            entity.WalmartId = request.WalmartId;

            try
            {
                var itemResponse = _walmartApiService.GetItem(request.WalmartId);

                var serializedItemResponse = JsonConvert.SerializeObject(itemResponse);

                //always update values from walmart to keep synced
                entity.WalmartItemResponse = serializedItemResponse;
                entity.Name = itemResponse.name;
                entity.ProductStock.Name = itemResponse.name;
                entity.Price = itemResponse.salePrice;
                entity.WalmartSize = itemResponse.size;
                entity.WalmartLink = string.Format("https://walmart.com/ip/{0}/{1}", itemResponse.name, itemResponse.itemId);
            }
            catch (Exception ex)
            {
                entity.Error = ex.Message;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}
