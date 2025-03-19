using Common.Models;
using OrderService.Models;

namespace OrderService.Services.Interfaces;

public interface IOrdersService
{
    Task OrderUpdateAsync(int orderId, string orderStatus, string? rejectionReason = null);
    Task<Order> CreateOrderRequestAsync(OrderRequestDto orderRequestDto);
    Task<Order> GetOrderInfoAsync(int orderRequestId);
}