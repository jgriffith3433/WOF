namespace WOF.Application.Products.Queries.GetProducts;

public class GetProductsVm
{
    public IList<ProductBriefDto> Products { get; set; } = new List<ProductBriefDto>();
}
