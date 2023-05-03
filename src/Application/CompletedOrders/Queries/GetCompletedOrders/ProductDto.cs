using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;
using WOF.Domain.Enums;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? WalmartId { get; set; }
    public string? WalmartLink { get; set; }
    public string? WalmartSize { get; set; }
    public string? WalmartItemResponse { get; set; }
    public string? Error { get; set; }
    public float Size { get; set; }
    public float Price { get; set; }
    public bool Verified { get; set; }
    public int SizeType { get; set; }
    //public SizeType SizeType { get; set; }
    public IList<CompletedOrder> CompletedOrders { get; private set; } = new List<CompletedOrder>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
            .ForMember(d => d.SizeType, opt => opt.MapFrom(s => (int)s.SizeType));
    }
}
