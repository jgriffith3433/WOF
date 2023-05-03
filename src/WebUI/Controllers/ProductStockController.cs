using WOF.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.ProductStocks.Queries;
using WOF.Application.ProductStocks.Queries.GetProductStock;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ProductStockController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetProductStockVm>> GetProductStock()
    {
        return await Mediator.Send(new GetProductStockQuery());
    }
}
