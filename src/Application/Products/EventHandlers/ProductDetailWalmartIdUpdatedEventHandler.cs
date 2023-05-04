using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using WOF.Domain.Entities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Products.EventHandlers;

public class ProductDetailWalmartIdUpdatedEventHandler : INotificationHandler<ProductDetailWalmartIdUpdatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ProductDetailWalmartIdUpdatedEventHandler> _logger;

    public ProductDetailWalmartIdUpdatedEventHandler(ILogger<ProductDetailWalmartIdUpdatedEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(ProductDetailWalmartIdUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);
        
        if (notification.Product.WalmartId != null)
        {
            try
            {
                var itemRequest = new ItemRequest
                {
                    id = notification.Product.WalmartId.ToString()
                };

                var itemResponse = itemRequest.GetResponse<ItemResponse>().Result;
                notification.Product.WalmartItemResponse = JsonConvert.SerializeObject(itemResponse);
                if (itemResponse.size == null)
                {
                    notification.Product.WalmartSize = "No walmart size";
                }
                else
                {
                    notification.Product.WalmartSize = itemResponse.size;
                }
                notification.Product.Price = itemResponse.salePrice;
                notification.Product.Name = itemResponse.name;
                notification.Product.WalmartLink = string.Format("https://www.walmart.com/ip/{0}/{1}", notification.Product.Name, notification.Product.WalmartId);
                notification.Product.Verified = true;
            }
            catch (Exception ex)
            {
                notification.Product.Error = ex.Message;
            }
        }
        return _context.SaveChangesAsync(cancellationToken);
    }
}
