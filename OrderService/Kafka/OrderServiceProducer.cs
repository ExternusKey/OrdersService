using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.Config;

namespace OrderService.Kafka;

public class OrderServiceProducer
{
    private readonly ILogger<OrderServiceProducer> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly string _topicName;

    public OrderServiceProducer(ILogger<OrderServiceProducer> logger, IOptions<ProducersConfig> producersConfig)
    {
        _topicName = producersConfig.Value.TopicName;
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = producersConfig.Value.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendOrderInfoAsync(string orderRequestJson)
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = orderRequestJson
        };
        await _producer.ProduceAsync(_topicName, message);
        _logger.LogInformation("Order request sent to Kafka");
    }
}