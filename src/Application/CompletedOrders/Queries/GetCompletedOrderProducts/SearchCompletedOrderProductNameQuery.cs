using AutoMapper;
using WOF.Application.Common.Interfaces;
using MediatR;
using WOF.Application.Common.Security;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrderProducts;

[Authorize]
public record SearchCompletedOrderProductNameQuery : IRequest<CompletedOrderProductDto>
{
    public int Id { get; init; }
    public string Search { get; init; }
}

public class SearchCompletedOrderProductNameQueryHandler : IRequestHandler<SearchCompletedOrderProductNameQuery, CompletedOrderProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchCompletedOrderProductNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrderProductDto> Handle(SearchCompletedOrderProductNameQuery request, CancellationToken cancellationToken)
    {
        var completedOrderProduct = await _context.CompletedOrderProducts.FirstOrDefaultAsync(cop => cop.Id == request.Id);

        var searchRequest = new SearchRequest
        {
            query = request.Search
        };

        var searchResponse = searchRequest.GetResponse<SearchResponse>().Result;
        completedOrderProduct.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CompletedOrderProductDto>(completedOrderProduct);
    }
}
