using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.Products.Commands.CreateProduct;
using WOF.Application.Products.Commands.UpdateProduct;
using WOF.Application.Products.Queries;
using WOF.Application.Products.Queries.GetProducts;
using WOF.Application.ProductStocks.Queries.GetProductStocks;
using WOF.Application.ProductStocks.Queries;
using WOF.Application.ProductStocks.Commands.DeleteProductStock;
using WOF.Application.Products.Commands.DeleteProduct;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetProductsVm>> GetProducts()
    {
        return await Mediator.Send(new GetProductsQuery());
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("UpdateProductName/{id}")]
    public async Task<ActionResult<ProductDto>> UpdateName(int id, UpdateProductNameCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("GetProductDetails")]
    public async Task<ActionResult<ProductDto>> GetProductDetails([FromQuery] GetProductDetailsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }


}
