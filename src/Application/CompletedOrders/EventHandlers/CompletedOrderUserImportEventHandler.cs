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
using WOF.Application.Common.Exceptions;
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
            var product = _context.Products.FirstOrDefaultAsync(i => i.Name == userImportObject.Name, cancellationToken).Result;
            if (product != null)
            {
                notification.CompletedOrder.Products.Add(product);
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
                notification.CompletedOrder.Products.Add(product);
            }
        }
        var result = _context.SaveChangesAsync(cancellationToken).Result;

        foreach (var product in notification.CompletedOrder.Products)
        {
            try
            {
                //var stock = _context.ProductStocks.FirstOrDefaultAsync(ps => ps.Product == product, cancellationToken).Result;
                //if (stock == null)
                //{
                //    stock = new ProductStock
                //    {
                //        Name = product.Name,
                //        Product = product
                //    };
                //    _context.ProductStocks.Add(stock);
                //}

                ////TODO: Need to convert from walmart product to stock units.
                ////for now just going to put 1 (even though we sometimes order X2 of products)
                ////Lots of parsing needs to be done using walmartresponse objects name and size properties
                ////stock.Units += product.Units;
                //stock.Units += 1;

                var itemRequest = new ItemRequest
                {
                    id = product.WalmartId.ToString()
                };

                var itemResponse = itemRequest.GetResponse<ItemResponse>().Result;
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
            result = _context.SaveChangesAsync(cancellationToken).Result;
        }
        
        return _context.SaveChangesAsync(cancellationToken);
    }
}
