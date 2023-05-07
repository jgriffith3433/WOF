using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using Newtonsoft.Json;
using AutoMapper;
using WOF.Application.Products.Queries;

namespace WOF.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Name = request.Name
        };

        //always ensure a product stock record exists for each product
        var productStock = new ProductStock
        {
            Name = request.Name,
            Units = 1
        };
        _context.ProductStocks.Add(productStock);

        productStock.Product = entity;

        var searchRequest = new SearchRequest
        {
            query = request.Name
        };

        var searchResponse = searchRequest.GetResponse<SearchResponse>().Result;
        entity.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}
