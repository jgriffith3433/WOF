using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.Products.Queries.GetProducts;

[Authorize]
public record GetProductsQuery : IRequest<GetProductsVm>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetProductsVm> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return new GetProductsVm
        {
            Products = await _context.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
