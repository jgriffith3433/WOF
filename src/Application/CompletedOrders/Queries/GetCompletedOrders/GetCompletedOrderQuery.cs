using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

[Authorize]
public record GetCompletedOrderQuery : IRequest<CompletedOrderDto>
{
    public int Id { get; set; }
}

public class GetCompletedOrderQueryHandler : IRequestHandler<GetCompletedOrderQuery, CompletedOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompletedOrderQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrderDto> Handle(GetCompletedOrderQuery request, CancellationToken cancellationToken)
    {
        return await _context.CompletedOrders.AsNoTracking().ProjectTo<CompletedOrderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
    }
}
