using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Products;
using WOF.Domain.Enums;

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
            UnitTypes = Enum.GetValues(typeof(UnitType))
                .Cast<UnitType>()
                .Select(p => new UnitTypeDto { Value = (int)p, Name = p.ToString() })
                .ToList(),

            ProductStocks = await _context.ProductStocks
                        .Include(ps => ps.Product)
                        .AsNoTracking()
                        .ProjectTo<ProductStockDto>(_mapper.ConfigurationProvider)
                        .OrderBy(t => t.Id)
                        .ToListAsync(cancellationToken)
        };
    }
}
