using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.Products.Queries.GetProducts;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetProductsVm>> GetProducts()
    {
        return await Mediator.Send(new GetProductsQuery());
    }
}
