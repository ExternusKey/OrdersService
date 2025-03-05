using System.Text.Json;
using Common.Models;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using ProcessingService.Kafka;
using ProcessingService.Models;
using ProcessingService.Models.Responses;
using ProcessingService.Services.Interfaces;

namespace ProcessingService.Services;

public class ProductsService(OrderDbContext dbContext, ProcessingServiceProducer producer) : IProductsService
{
    public async Task<ProductResponse> CheckProductAvailabilityAsync(int productId)
    {
        var product = await dbContext.Products
            .SingleOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return new ProductResponse
            {
                Status = ProductResponseStatus.ProductNotFound,
                Message = "Product not found"
            };
        }

        return new ProductResponse
        {
            Status = ProductResponseStatus.Success
        };
    }

    public async Task<ProductResponse> UpdateProductAmountAsync(int productId, int amount)
    {
        var product = await dbContext.Products
            .SingleOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            return new ProductResponse { Status = ProductResponseStatus.ProductNotFound, Message = "Product not found" };

        if (amount > product.Amount)
            return new ProductResponse { Status = ProductResponseStatus.NotEnoughAmount, Message = "Not enough product amount" };
    
        product.Amount -= amount;
        await dbContext.SaveChangesAsync();
        return new ProductResponse 
        { 
            Status = ProductResponseStatus.Success 
        };
    }

    
    public async Task AddOrderAsync(Orders order, bool status, string? reasonMsg = null)
    {
        if (status)
        {
            var confirmedOrder = new ConfirmedOrder
            {
                OrderId = order.Id,
                OrderStatus = "Confirmed",
                OrderConfirmationDate = DateTime.UtcNow
            };
            dbContext.ConfirmedOrders.Add(confirmedOrder);
            await dbContext.SaveChangesAsync();
            
            Console.WriteLine($"[ProcessingService] Order: {order.Id} confirmed and saved.");
            
            var orderStatusConfirmation = new OrderStatusConfirmationResponse
            {
                OrderId = order.Id,
                OrderStatus = "Confirmed",
                StatusChangeDate = DateTime.UtcNow
            };
            var orderConfirmation = JsonSerializer.Serialize(orderStatusConfirmation);
            await producer.SendStatusOrderAsync(orderConfirmation);
        }
        else
        {
            var rejectedOrder = new RejectedOrder()
            {
                OrderId = order.Id,
                RejectionReason = reasonMsg,
                OrderRejectionDate = DateTime.UtcNow
            };
        
            dbContext.RejectedOrders.Add(rejectedOrder);
            await dbContext.SaveChangesAsync();
            
            Console.WriteLine($"[ProcessingService] Order: {order.Id} rejected and saved.");
            
            var orderStatusConfirmation = new OrderStatusConfirmationResponse
            {
                OrderId = order.Id,
                OrderStatus = "Rejected",
                RejectionReason = reasonMsg,
                StatusChangeDate = DateTime.UtcNow
            };
            
            var orderRejection = JsonSerializer.Serialize(orderStatusConfirmation);
            await producer.SendStatusOrderAsync(orderRejection);
        }
    }
}
