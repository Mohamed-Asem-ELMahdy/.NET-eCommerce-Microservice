using Application.Interfaces;
using Application.Services;
using Domain.DTOs;
using eCommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace Presentaion.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService, IOrder orderInterface) : ControllerBase
{
    // GET api/orders/client/5
    [HttpGet("client/{clientId:int}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByClient(int clientId)
    {
        var orders = await orderService.GetOrdersByClient(clientId);
        return Ok(orders);
    }

    // GET api/orders/5
    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(int orderId)
    {
        var details = await orderService.GetOrderDetails(orderId);
        if (details is null)
            return NotFound(new { Message = $"Order {orderId} not found." });

        return Ok(details);
    }

    // POST api/orders
    [HttpPost]
    public async Task<ActionResult<Response>> PlaceOrder([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = new Domain.Entities.Order
        {
            ProductId       = orderDto.ProductId,
            ClientId        = orderDto.ClientId,
            Quantity        = orderDto.PurchaseQuantity,
            Date            = DateTime.UtcNow
        };

        var response = await orderInterface.CreateAsync(order);
        return response.Flag
            ? CreatedAtAction(nameof(GetOrderDetails), new { orderId = order.Id }, response)
            : BadRequest(response);
    }

    // PUT api/orders
    [HttpPut]
    public async Task<ActionResult<Response>> UpdateOrder([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = new Domain.Entities.Order
        {
            Id       = orderDto.Id,
            ProductId= orderDto.ProductId,
            ClientId = orderDto.ClientId,
            Quantity = orderDto.PurchaseQuantity,
            Date     = orderDto.Date
        };

        var response = await orderInterface.UpdateAsync(order);
        return response.Flag ? Ok(response) : BadRequest(response);
    }

    // DELETE api/orders
    [HttpDelete]
    public async Task<ActionResult<Response>> DeleteOrder([FromBody] OrderDto orderDto)
    {
        var order = new Domain.Entities.Order { Id = orderDto.Id };
        var response = await orderInterface.DeleteAsync(order);
        return response.Flag ? Ok(response) : BadRequest(response);
    }
}
