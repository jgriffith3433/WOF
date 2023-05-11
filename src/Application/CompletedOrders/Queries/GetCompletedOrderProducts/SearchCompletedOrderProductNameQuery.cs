using AutoMapper;
using WOF.Application.Common.Interfaces;
using MediatR;
using WOF.Application.Common.Security;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
    private readonly IWalmartApiService _walmartApiService;

    public SearchCompletedOrderProductNameQueryHandler(IApplicationDbContext context, IMapper mapper, IWalmartApiService walmartApiService)
    {
        _context = context;
        _mapper = mapper;
        _walmartApiService = walmartApiService;
    }

    public async Task<CompletedOrderProductDto> Handle(SearchCompletedOrderProductNameQuery request, CancellationToken cancellationToken)
    {
        var completedOrderProduct = await _context.CompletedOrderProducts.FirstOrDefaultAsync(cop => cop.Id == request.Id);

        var searchResponse = _walmartApiService.Search(request.Search);

        completedOrderProduct.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CompletedOrderProductDto>(completedOrderProduct);
    }
}
