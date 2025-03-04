namespace ProcessingService.Models.Responses;

public class ProductResponse
{
    public ProductResponseStatus Status { get; set; }
    public string? Message { get; set; }

    public bool IsSuccess => Status == ProductResponseStatus.Success;
}