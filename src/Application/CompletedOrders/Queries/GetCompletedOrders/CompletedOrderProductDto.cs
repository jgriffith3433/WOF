using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Application.Products.Queries;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrderProductDto : IMapFrom<CompletedOrderProduct>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long? WalmartId { get; set; }
    public string? WalmartItemResponse { get; set; }
    public string? WalmartSearchResponse { get; set; }
    public string? WalmartError { get; set; }
    public int CompletedOrderId { get; set; }

    public ProductDto? Product { get; set; }
}
