namespace ProcessingService.Models.Responses;

public class OrderStatusConfirmationResponse
{
    public int OrderId { get; set; }
    public string OrderStatus { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime StatusChangeDate { get; set; }

}