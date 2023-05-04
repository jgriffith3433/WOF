using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.Products.Queries.GetProducts;

[Authorize]
public record GetProductQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products.AsNoTracking().ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
    }
}
