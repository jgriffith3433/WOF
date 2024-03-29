﻿using AutoMapper;
using MediatR;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Models.Walmart.Responses;
using WOF.Application.Common.Security;

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
    private readonly IWalmartApiService _walmartApiService;

    public ProductLookupQueryHandler(IApplicationDbContext context, IMapper mapper, IWalmartApiService walmartApiService)
    {
        _context = context;
        _mapper = mapper;
        _walmartApiService = walmartApiService;
    }

    public async Task<ItemResponse> Handle(ProductLookupQuery productLookupQuery, CancellationToken cancellationToken)
    {
        return _walmartApiService.GetItem(productLookupQuery.Id);
    }
}
