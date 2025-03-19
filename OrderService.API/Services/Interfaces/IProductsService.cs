using Common.Models;
using OrderService.Models;

namespace OrderService.Services.Interfaces;

public interface IProductsService
{
    Task<List<Product>> GetProductsAsync();
    Task<int> AddProductAsync(ProductRequestDto productRequestDto);
}