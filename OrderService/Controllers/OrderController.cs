using Microsoft.AspNetCore.Mvc;
using OrderService.Exceptions;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(OrdersService ordersService)
    : Controller
{
    [HttpPost("CreateOrder")] 
    public async Task<IActionResult> CreateOrderRequestAsync([FromBody] OrderRequestDto orderRequestDto)
    {
        try
        {
            var orderRequestId = await ordersService.CreateOrderRequestAsync(orderRequestDto);
            return Ok(new { orderRequestId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpGet("GetOrderInfo")]
    public async Task<IActionResult> GetInfoAboutOrderAsync([FromQuery] int orderRequestId)
    {
        try
        {
            var orderInfo = await ordersService.GetOrderInfoAsync(orderRequestId);
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