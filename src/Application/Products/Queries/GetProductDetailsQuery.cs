using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using MediatR;
using WOF.Application.Common.Security;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Products.Queries;

namespace WOF.Application.ProductStocks.Queries.GetProductStocks;

[Authorize]
public record GetProductDetailsQuery : IRequest<ProductDto>
{
    public int Id { get; init; }
}

public class GetProductStockQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStockQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products.AsNoTracking().Include(p => p.ProductStock).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);
    }
}
