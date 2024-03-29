﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.ProductStocks.Queries;
using WOF.Application.ProductStocks.Queries.GetProductStocks;
using WOF.Application.ProductStocks.Commands.UpdateProductStock;
using WOF.Application.ProductStocks.Commands.CreateProductStock;
using WOF.Application.ProductStocks.Commands.DeleteProductStock;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ProductStockController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetProductStocksVm>> GetProductStocks()
    {
        return await Mediator.Send(new GetProductStocksQuery());
    }

    [HttpPut]
    public async Task<ActionResult<ProductStockDto>> Update(int id, UpdateProductStockCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("GetProductStockDetails")]
    public async Task<ActionResult<ProductStockDetailsVm>> GetProductStockDetails([FromQuery] GetProductStockDetailsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<ProductStockDto>> UpdateProductStockDetails(int id, UpdateProductStockDetailsCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateProductStockCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductStockCommand(id));

        return NoContent();
    }


}
