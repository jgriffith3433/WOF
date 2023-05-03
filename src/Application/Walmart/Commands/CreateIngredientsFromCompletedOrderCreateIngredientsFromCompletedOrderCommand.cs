using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Walmart.Queries;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Exceptions;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace WOF.Application.Walmart.Commands;


public record CreateProductsFromCompletedOrderCommand : IRequest<CompletedOrderDto>
{
    public int CompletedOrderId { get; init; }
}

public class CreateProductsFromCompletedOrderCommandHandler : IRequestHandler<CreateProductsFromCompletedOrderCommand, CompletedOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductsFromCompletedOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrderDto> Handle(CreateProductsFromCompletedOrderCommand request, CancellationToken cancellationToken)
    {
        var completedOrder = await _context.CompletedOrders
            .FindAsync(new object[] { request.CompletedOrderId }, cancellationToken);

        if (completedOrder == null)
        {
            throw new NotFoundException(nameof(CompletedOrder), request.CompletedOrderId);
        }

        var userImportObjects = JsonConvert.DeserializeObject<List<UserImportObject>>(completedOrder.UserImport);

        foreach (var userImportObject in userImportObjects)
        {
            var splitLink = userImportObject.Link.Split('/');
            int walmartId = int.Parse(splitLink[splitLink.Length - 1]);
            var existingProduct = await _context.Products.FirstOrDefaultAsync(i => i.Name == userImportObject.Name && i.WalmartId == walmartId);
            if (existingProduct != null)
            {
                completedOrder.Products.Add(existingProduct);
            }
            else
            {
                var entity = new Product
                {
                    Name = userImportObject.Name,
                    //Link = userImportObject.Link,
                    WalmartId = walmartId
                };
                _context.Products.Add(entity);
                completedOrder.Products.Add(entity);
            }
        }
        await _context.SaveChangesAsync(cancellationToken);

        //var itemRequest = new ItemRequest
        //{
        //    ids = productLookupQuery.Id
        //};

        //var itemResponse = await itemRequest.GetResponse<ItemResponse>();

        return _mapper.Map<CompletedOrderDto>(completedOrder);
    }
}

