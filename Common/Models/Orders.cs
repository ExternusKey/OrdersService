using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

[Table("Orders")]
[Microsoft.EntityFrameworkCore.Index(nameof(ProductId))]
public class Orders
{
    [Column("order_id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("order_status")]
    public string Status { get; set; }
    
    [Column("order_rejected_reason")]
    public string? RejectedReason { get; set; }

    [Column("order_created_date")]
    public DateTime OrderDate { get; set; }
}
