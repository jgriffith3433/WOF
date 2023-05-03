using AutoMapper;
using MediatR;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;

namespace WOF.Application.Walmart.Queries;


[Authorize]
public record ProductLookupQuery : IRequest<ItemResponse>
{
    public int Id { get; set; }
}

public class ProductLookupQueryHandler : IRequestHandler<ProductLookupQuery, ItemResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductLookupQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ItemResponse> Handle(ProductLookupQuery productLookupQuery, CancellationToken cancellationToken)
    {
        var itemRequest = new ItemRequest
        {
            ids = productLookupQuery.Id
        };
        return await itemRequest.GetResponse<ItemResponse>();
    }
}
