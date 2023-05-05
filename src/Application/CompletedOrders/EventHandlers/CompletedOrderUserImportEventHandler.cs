using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using Newtonsoft.Json;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.Products.EventHandlers;

public class CompletedOrderUserImportEventHandler : INotificationHandler<CompletedOrderUserImportEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CompletedOrderUserImportEventHandler> _logger;

    public CompletedOrderUserImportEventHandler(ILogger<CompletedOrderUserImportEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(CompletedOrderUserImportEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);


        var userImportObjects = JsonConvert.DeserializeObject<List<UserImportObject>>(notification.CompletedOrder.UserImport);

        foreach (var userImportObject in userImportObjects)
        {
            var splitLink = userImportObject.Link.Split('/');
            int walmartId = int.Parse(splitLink[splitLink.Length - 1]);

            var completedOrderProduct = new CompletedOrderProduct
            {
                Name = userImportObject.Name,
                WalmartId = walmartId
            };
            _context.CompletedOrderProducts.Add(completedOrderProduct);

            notification.CompletedOrder.CompletedOrderProducts.Add(completedOrderProduct);
        }
        var result = _context.SaveChangesAsync(cancellationToken).Result;

        foreach (var completedOrderProduct in notification.CompletedOrder.CompletedOrderProducts)
        {
            try
            {
                var itemRequest = new ItemRequest
                {
                    id = completedOrderProduct.WalmartId.ToString()
                };

                var itemResponse = itemRequest.GetResponse<ItemResponse>().Result;
                var serializedItemResponse = JsonConvert.SerializeObject(itemResponse);
                completedOrderProduct.WalmartItemResponse = serializedItemResponse;
                completedOrderProduct.Name = itemResponse.name;

                var productEntity = _context.Products.FirstOrDefault(p => p.WalmartId == itemResponse.itemId);
                ProductStock? productStock = null;

                if (productEntity != null)
                {
                    productStock = _context.ProductStocks.FirstOrDefault(ps => ps.Product == productEntity);
                    productStock.Units += 1;
                }
                else
                {
                    productEntity = new Product
                    {
                        WalmartId = itemResponse.itemId,
                    };

                    //always ensure a product stock record exists for each product
                    productStock = new ProductStock
                    {
                        Name = itemResponse.name,
                        Units = 1
                    };
                    _context.ProductStocks.Add(productStock);

                    //TODO: do we only need one of these for entity framework?
                    productStock.Product = productEntity;
                }

                //always update values from walmart to keep synced
                productEntity.WalmartItemResponse = serializedItemResponse;
                productEntity.Name = itemResponse.name;
                productEntity.Price = itemResponse.salePrice;
                productEntity.WalmartSize = itemResponse.size;
                productEntity.WalmartLink = string.Format("https://walmart.com/ip/{0}/{1}", itemResponse.name, itemResponse.itemId);
                completedOrderProduct.Product = productEntity;
            }
            catch (Exception ex)
            {
                completedOrderProduct.WalmartError = ex.Message;
            }
            result = _context.SaveChangesAsync(cancellationToken).Result;
        }

        return _context.SaveChangesAsync(cancellationToken);
    }
}
