using Common.Models;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Exceptions;
using OrderService.Models;
using OrderService.Services.Interfaces;

namespace OrderService.Services;

public class ProductsService(OrderDbContext dbContext) : IProductsService
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var products = await dbContext.Products.ToListAsync();
    
        if (products == null || products.Count == 0)
        {
            throw new ProductNotFoundException();
        }

        return products;
    }

    public async Task<int> AddProductAsync(ProductRequestDto productRequestDto)
    {    
        if (productRequestDto == null)
        {
            throw new ProductCreationException();
        }
        var product = new Product
        {
            Name = productRequestDto.Name,
            Amount = productRequestDto.Amount,
        };
        
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product.Id;
    }
}
