using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class OrderRequestDto(string productName, int quantity)
{
    [Required(ErrorMessage = "Product name is required")]
    public string ProductName{get;} = productName;

    [Required(ErrorMessage = "Quantity is required")]
    public int Quantity{get;} = quantity;
}