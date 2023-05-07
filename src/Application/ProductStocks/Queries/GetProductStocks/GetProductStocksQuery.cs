using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.ProductStocks.Queries.GetProductStocks;

[Authorize]
public record GetProductStocksQuery : IRequest<GetProductStocksVm>;

public class GetProductStocksQueryHandler : IRequestHandler<GetProductStocksQuery, GetProductStocksVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStocksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetProductStocksVm> Handle(GetProductStocksQuery request, CancellationToken cancellationToken)
    {
        return new GetProductStocksVm
        {
            ProductStocks = await _context.ProductStocks
                .AsNoTracking()
                .ProjectTo<ProductStockDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
