namespace OrderService.Models;

public class OrderConfirmation
{
    public string OrderId { get; set; }
    public string OrderNumber { get; set; }
    public string OrderStatus { get; set; }
    public string OrderDate { get; set; }
}