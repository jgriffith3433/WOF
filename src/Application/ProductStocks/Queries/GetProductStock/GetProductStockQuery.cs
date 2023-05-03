using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.ProductStocks.Queries.GetProductStock;

[Authorize]
public record GetProductStockQuery : IRequest<GetProductStockVm>;

public class GetProductStockQueryHandler : IRequestHandler<GetProductStockQuery, GetProductStockVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStockQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetProductStockVm> Handle(GetProductStockQuery request, CancellationToken cancellationToken)
    {
        return new GetProductStockVm
        {
            ProductStocks = await _context.ProductStocks
                .AsNoTracking()
                .ProjectTo<ProductStockDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
