using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using MediatR;
using WOF.Application.Common.Security;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WOF.Application.Products.Queries;

namespace WOF.Application.ProductStocks.Queries.GetProductStocks;

[Authorize]
public record GetProductStockDetailsQuery : IRequest<ProductStockDetailsVm>
{
    public int Id { get; init; }
    public string Search { get; init; }
}

public class GetProductStockDetailsQueryHandler : IRequestHandler<GetProductStockDetailsQuery, ProductStockDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStockDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductStockDetailsVm> Handle(GetProductStockDetailsQuery request, CancellationToken cancellationToken)
    {
        var productStockDetails = await _context.ProductStocks.AsNoTracking().ProjectTo<ProductStockDetailsVm>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);

        var query = from ps in _context.Products
                    where EF.Functions.Like(ps.Name, string.Format("%{0}%", request.Search))
                    select ps;

        productStockDetails.ProductSearchItems = query.Include(p => p.ProductStock).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToList();

        foreach(var productSearchItem in productStockDetails.ProductSearchItems)
        {
            if (productSearchItem.ProductStockId != null)
            {
                productSearchItem.Name += " ( Merge )";
            }
        }

        return productStockDetails;
    }
}
