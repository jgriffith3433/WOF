using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrderProducts;

[Authorize]
public record GetCompletedOrderProductsQuery : IRequest<GetCompletedOrderProductsVm>;

public class GetCompletedOrderProductsQueryHandler : IRequestHandler<GetCompletedOrderProductsQuery, GetCompletedOrderProductsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompletedOrderProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCompletedOrderProductsVm> Handle(GetCompletedOrderProductsQuery request, CancellationToken cancellationToken)
    {
        return new GetCompletedOrderProductsVm
        {
            Products = await _context.CompletedOrderProducts
                .AsNoTracking()
                .ProjectTo<CompletedOrderProductDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
