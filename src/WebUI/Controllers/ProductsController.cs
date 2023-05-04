using WOF.Application.Common.Models;
using WOF.Application.Products.Commands.CreateProduct;
using WOF.Application.Products.Commands.DeleteProduct;
using WOF.Application.Products.Commands.UpdateProduct;
using WOF.Application.Products.Commands.UpdateProductDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.Products.Queries;
using WOF.Application.Products.Queries.GetProducts;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        return await Mediator.Send(new GetProductQuery
        {
            Id = id
        });
    }

    [HttpGet]
    public async Task<ActionResult<GetProductsVm>> GetProducts()
    {
        return await Mediator.Send(new GetProductsQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<ActionResult> UpdateProductDetails(int id, UpdateProductDetailCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }
}
