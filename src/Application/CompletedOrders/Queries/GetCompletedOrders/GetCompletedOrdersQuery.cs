using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using WOF.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

[Authorize]
public record GetCompletedOrdersQuery : IRequest<CompletedOrdersVm>;

public class GetCompletedOrdersQueryHandler : IRequestHandler<GetCompletedOrdersQuery, CompletedOrdersVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompletedOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrdersVm> Handle(GetCompletedOrdersQuery request, CancellationToken cancellationToken)
    {
        return new CompletedOrdersVm
        {
            CompletedOrders = await _context.CompletedOrders
                .AsNoTracking()
                .ProjectTo<CompletedOrderDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
