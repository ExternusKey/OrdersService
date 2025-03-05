using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using OrderService.Exceptions;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(OrdersService ordersService)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateOrderRequestAsync([FromBody] OrderRequestDto orderRequestDto)
    {
        try
        {
            var orderRequestId = await ordersService.CreateOrderRequestAsync(orderRequestDto);
            return StatusCode(StatusCodes.Status201Created, orderRequestId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpGet("{id}", Name = "GetInfoAboutOrderAsync")]
    public async Task<IActionResult> GetInfoAboutOrderAsync(int id)
    {
        try
        {
            var orderInfo = await ordersService.GetOrderInfoAsync(id);
            return Ok(new { orderInfo });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });  
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

}