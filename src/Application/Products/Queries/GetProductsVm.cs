namespace WOF.Application.Products.Queries.GetProducts;

public class GetProductsVm
{
    public IList<SizeTypeDto> SizeTypes { get; set; } = new List<SizeTypeDto>();
    public IList<ProductDto> Products { get; set; } = new List<ProductDto>();
}
