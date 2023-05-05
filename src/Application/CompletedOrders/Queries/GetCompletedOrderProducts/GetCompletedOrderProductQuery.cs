using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrderProducts;

[Authorize]
public record GetCompletedOrderProductQuery : IRequest<CompletedOrderProductDto>
{
    public int Id { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetCompletedOrderProductQuery, CompletedOrderProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrderProductDto> Handle(GetCompletedOrderProductQuery request, CancellationToken cancellationToken)
    {
        return await _context.CompletedOrderProducts.AsNoTracking().ProjectTo<CompletedOrderProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
    }
}
