namespace WOF.Application.Products.Queries.GetProducts;

public class GetProductsVm
{
    public IList<UnitTypeDto> UnitTypes { get; set; } = new List<UnitTypeDto>();
    public IList<ProductDto> Products { get; set; } = new List<ProductDto>();
}
