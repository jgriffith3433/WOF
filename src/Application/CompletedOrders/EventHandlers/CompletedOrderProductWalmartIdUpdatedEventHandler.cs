using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Interfaces;
using Newtonsoft.Json;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.EventHandlers;

public class CompletedOrderProductWalmartIdUpdatedEventHandler : INotificationHandler<CompletedOrderProductWalmartIdUpdatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CompletedOrderProductWalmartIdUpdatedEventHandler> _logger;

    public CompletedOrderProductWalmartIdUpdatedEventHandler(ILogger<CompletedOrderProductWalmartIdUpdatedEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(CompletedOrderProductWalmartIdUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        if (notification.CompletedOrderProduct.WalmartId != null)
        {
            try
            {
                var itemRequest = new ItemRequest
                {
                    id = notification.CompletedOrderProduct.WalmartId.ToString()
                };

                var itemResponse = itemRequest.GetResponse<ItemResponse>().Result;
                var serializedItemResponse = JsonConvert.SerializeObject(itemResponse);
                notification.CompletedOrderProduct.WalmartItemResponse = serializedItemResponse;
                notification.CompletedOrderProduct.Name = itemResponse.name;

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
                notification.CompletedOrderProduct.Product = productEntity;
            }
            catch (Exception ex)
            {
                notification.CompletedOrderProduct.WalmartError = ex.Message;
            }
        }
        return _context.SaveChangesAsync(cancellationToken);
    }
}
