using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class ProductRequestDto
{
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Amount must be not null")]
    public int Amount { get; set; }
}