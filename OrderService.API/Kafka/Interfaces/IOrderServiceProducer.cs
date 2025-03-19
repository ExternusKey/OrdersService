namespace OrderService.Kafka.Interfaces;

public interface IOrderServiceProducer
{
    Task SendOrderInfoAsync(string orderRequestJson);
}