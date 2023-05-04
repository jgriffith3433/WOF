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
using System.Numerics;
using Org.BouncyCastle.Crypto;

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
            var product = await _context.Products.FirstOrDefaultAsync(i => i.Name == userImportObject.Name && i.WalmartId == walmartId, cancellationToken);
            if (product != null)
            {
                completedOrder.Products.Add(product);
            }
            else
            {
                product = new Product
                {
                    Name = userImportObject.Name,
                    WalmartId = walmartId,
                    WalmartLink = userImportObject.Link
                };
                _context.Products.Add(product);
                completedOrder.Products.Add(product);
            }
        }
        await _context.SaveChangesAsync(cancellationToken);

        //if (completedOrder.Products.Count == 1)
        //{
        foreach (var product in completedOrder.Products)
        {
            try
            {
                var stock = await _context.ProductStocks.FirstOrDefaultAsync(ps => ps.Product == product, cancellationToken);
                if (stock == null)
                {
                    stock = new ProductStock
                    {
                        Name = product.Name,
                        Product = product
                    };
                    _context.ProductStocks.Add(stock);
                }

                //TODO: Need to convert from walmart product to stock units.
                //for now just going to put 1 (even though we sometimes order X2 of products)
                //Lots of parsing needs to be done using walmartresponse objects name and size properties
                //stock.Units += product.Units;
                stock.Units += 1;

                var itemRequest = new ItemRequest
                {
                    id = product.WalmartId.ToString()
                };

                var itemResponse = await itemRequest.GetResponse<ItemResponse>();
                product.WalmartItemResponse = JsonConvert.SerializeObject(itemResponse);
                if (itemResponse.size == null)
                {
                    product.WalmartSize = "No walmart size";
                }
                else
                {
                    product.WalmartSize = itemResponse.size;
                }
                product.Price = itemResponse.salePrice;
            }
            catch (Exception ex)
            {
                product.Error = ex.Message;
            }
        }
        //}
        //else if (completedOrder.Products.Count > 1)
        //{
        //    //20 items lookup per request
        //    var currentProductIndex = 0;
        //    var ids = "";
        //    for (var i = 0; i < completedOrder.Products.Count; i++)
        //    {
        //        ids += completedOrder.Products[i].WalmartId + ",";
        //        currentProductIndex++;
        //        if (currentProductIndex == 2 || currentProductIndex == completedOrder.Products.Count - 1)
        //        {
        //            var multipleItemsRequest = new MultipleItemsRequest
        //            {
        //                ids = ids
        //            };

        //            var multipleItemResponse = await multipleItemsRequest.GetResponse<MultipleItemResponse>();
        //            for (var k = 0; k < currentProductIndex; k++)
        //            {
        //                var numberString = new string(multipleItemResponse.items[k].size.Where(c => char.IsDigit(c) || c == '.').ToArray());
        //                completedOrder.Products[k].Units = float.Parse(numberString);
        //                completedOrder.Products[k].Price = multipleItemResponse.items[k].salePrice;
        //            }
        //            currentProductIndex = 0;
        //        }
        //    }
        //}
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CompletedOrderDto>(completedOrder);
    }
}

