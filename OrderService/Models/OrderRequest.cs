using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models;

[Table("OrdersRequest")]
[Microsoft.EntityFrameworkCore.Index(nameof(Product))]
public class OrderRequest
{
    [Key]
    public Guid Id { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
}