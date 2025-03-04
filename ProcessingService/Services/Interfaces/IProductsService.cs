using Common.Models;
using ProcessingService.Models.Responses;

namespace ProcessingService.Services.Interfaces;

public interface IProductsService
{
    Task<ProductResponse> CheckProductAvailabilityAsync(int productId);
    Task<ProductResponse> UpdateProductAmountAsync(int productId, int amount);
    Task AddOrderAsync(Orders orders, bool status, string? reasonMsg = null);
}