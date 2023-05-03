using WOF.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.Stocks.Queries;
using WOF.Application.Stocks.Queries.GetStock;

namespace WOF.WebUI.Controllers;

[Authorize]
public class StockController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetStockVm>> GetStock()
    {
        return await Mediator.Send(new GetStockQuery());
    }
}
