using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(OrderDbContext dbContext, OrderServiceProducer kafkaProducer)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateOrderRequestAsync([FromBody] OrderRequestDto orderRequestDto)
    {
        var orderRequest = new OrderRequest()
        {
            Id = Guid.NewGuid(),
            Product = orderRequestDto.ProductName,
            Quantity = orderRequestDto.Quantity,
            OrderDate = DateTime.Now,
        };
        
        dbContext.OrderRequests.Add(orderRequest);
        await dbContext.SaveChangesAsync();

        var json = JsonSerializer.Serialize(orderRequest);
        await kafkaProducer.SendOrderInfoAsync(json);
        
        return Ok(new { orderRequestId = orderRequest.Id });
    }

    [HttpGet]
    public async Task<IActionResult> GetInfoAboutOrderAsync([FromBody] string orderRequestId)
    {
        // TODO : Получения инфы о заказе
        return Ok(await dbContext.OrderRequests.FindAsync(orderRequestId));
    }
}