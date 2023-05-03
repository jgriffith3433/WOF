using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Stocks.Queries.GetStock;

[Authorize]
public record GetStockQuery : IRequest<GetStockVm>;

public class GetStockQueryHandler : IRequestHandler<GetStockQuery, GetStockVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStockQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetStockVm> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        return new GetStockVm
        {
            Ingredients = await _context.Stocks
                .AsNoTracking()
                .ProjectTo<StockDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
