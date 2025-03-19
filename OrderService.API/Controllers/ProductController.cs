using Microsoft.AspNetCore.Mvc;
using OrderService.Exceptions;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(ProductsService productsService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await productsService.GetProductsAsync();
            return Ok(new { products });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProductAsync(ProductRequestDto productRequestDto)
    {
        try
        {
            var productId = await productsService.AddProductAsync(productRequestDto);
            return Ok(new { Id = productId });
        }
        catch (ProductCreationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }
}