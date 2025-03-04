using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

[Table("RejectedOrders")]
public class RejectedOrder
{
    [Column("rejection_id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("rejection_reason")]
    public string? RejectionReason { get; set; }

    [Column("order_rejection_date")]
    public DateTime OrderRejectionDate { get; set; }
}