using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Products.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;

namespace WOF.Application.Products.Commands.UpdateProduct;

public record UpdateProductNameCommand : IRequest<ProductDto>
{
    public int Id { get; init; }

    public string Name { get; init; }
}

public class UpdateProductNameCommandHandler : IRequestHandler<UpdateProductNameCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProductNameCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductNameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.Include(p => p.ProductStock).FirstOrDefaultAsync(ps => ps.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        if (entity.Name != request.Name && entity.WalmartId == null)
        {
            //Still searching for walmart product
            entity.Name = request.Name;
            entity.ProductStock.Name = request.Name;
            var searchRequest = new SearchRequest
            {
                query = request.Name
            };

            var searchResponse = searchRequest.GetResponse<SearchResponse>().Result;
            entity.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}
