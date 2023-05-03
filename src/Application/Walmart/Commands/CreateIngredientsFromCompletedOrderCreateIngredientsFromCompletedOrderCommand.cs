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


public record CreateIngredientsFromCompletedOrderCommand : IRequest<CompletedOrderDto>
{
    public int CompletedOrderId { get; init; }
}

public class CreateIngredientsFromCompletedOrderCommandHandler : IRequestHandler<CreateIngredientsFromCompletedOrderCommand, CompletedOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateIngredientsFromCompletedOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrderDto> Handle(CreateIngredientsFromCompletedOrderCommand request, CancellationToken cancellationToken)
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
            var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == userImportObject.Name && i.WalmartId == walmartId);
            if (existingIngredient != null)
            {
                completedOrder.Ingredients.Add(existingIngredient);
            }
            else
            {
                var entity = new Ingredient
                {
                    Name = userImportObject.Name,
                    //Link = userImportObject.Link,
                    WalmartId = walmartId
                };
                _context.Ingredients.Add(entity);
                completedOrder.Ingredients.Add(entity);
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

