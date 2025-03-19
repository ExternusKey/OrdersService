using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class OrderRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be not null")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be not null")]
    public int Quantity { get; set; }
}