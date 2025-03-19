using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

[Table("confirmed_orders")]
public class ConfirmedOrder
{
    [Column("confirmation_id")] public int Id { get; set; }

    [Column("order_number")] public int OrderId { get; set; }

    [Column("order_status")] public string OrderStatus { get; set; }

    [Column("order_confirmed_date")] public DateTime OrderConfirmationDate { get; set; }
}