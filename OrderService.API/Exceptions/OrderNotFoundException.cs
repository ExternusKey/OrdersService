namespace OrderService.Exceptions;

public class OrderNotFoundException(int orderRequestId)
    : Exception($"Order request with ID {orderRequestId} not found.");