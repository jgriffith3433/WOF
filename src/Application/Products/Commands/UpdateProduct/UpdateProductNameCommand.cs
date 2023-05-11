using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Products.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
    private readonly IWalmartApiService _walmartApiService;

    public UpdateProductNameCommandHandler(IApplicationDbContext context, IMapper mapper, IWalmartApiService walmartApiService)
    {
        _context = context;
        _mapper = mapper;
        _walmartApiService = walmartApiService;
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

            var searchResponse = _walmartApiService.Search(request.Name);

            entity.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}
