using System.Text.Json;
using Confluent.Kafka;
using OrderService.Config;
using OrderService.Models;

namespace OrderService.Kafka;

public class OrderServiceProducer
{
    private readonly IProducer<string, string> _producer;
    private const string TopicName = ProducersConfig.TopicName;

    public OrderServiceProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = ProducersConfig.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendOrderInfoAsync(string orderRequest)
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(orderRequest)
        };
        await _producer.ProduceAsync(TopicName, message);
        Console.WriteLine("[OrderService] Order request send to Kafka...");
    }
}