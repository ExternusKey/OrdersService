using System.Text.Json;
using Common.Models;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Exceptions;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Services.Interfaces;

namespace OrderService.Services;

public class OrdersService(OrderDbContext dbContext, OrderServiceProducer kafkaProducer, ILogger<OrdersService> _logger)
    : IOrdersService
{
    public async Task OrderUpdateAsync(int orderId, string orderStatus, string? rejectionReason = null)
    {
        var order = await dbContext.OrderRequests
            .SingleOrDefaultAsync(p => p.Id == orderId);

        if (order != null)
        {
            order.Status = orderStatus;
            order.RejectedReason = rejectionReason;
        }

        await dbContext.SaveChangesAsync();
        _logger.LogInformation($"Order {orderId} status has been updated.");
    }

    public async Task<Orders> CreateOrderRequestAsync(OrderRequestDto orderRequestDto)
    {
        var orderRequest = new Orders
        {
            ProductId = orderRequestDto.ProductId,
            Amount = orderRequestDto.Quantity,
            Status = "Pending",
            OrderDate = DateTime.UtcNow,
        };

        dbContext.OrderRequests.Add(orderRequest);
        await dbContext.SaveChangesAsync();
        _logger.LogInformation($"Order {orderRequest.Id} has been added.");

        var json = JsonSerializer.Serialize(orderRequest);
        await kafkaProducer.SendOrderInfoAsync(json);

        return orderRequest;
    }

    public async Task<Orders> GetOrderInfoAsync(int orderRequestId)
    {
        var order = await dbContext.OrderRequests.FindAsync(orderRequestId);

        if (order == null)
            throw new OrderNotFoundException(orderRequestId);
        _logger.LogInformation($"Order {orderRequestId} found.");
        return order;
    }
}