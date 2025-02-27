using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models;

[Table("products")]
public class Products
{
    [Key]
    public string Id { get; set; }
    [Column("product_name")]
    public string ProductName { get; set; }
    [Column("quantity")]
    public string Quantity { get; set; }
    [Column("price")]
    public string Price { get; set; }
}