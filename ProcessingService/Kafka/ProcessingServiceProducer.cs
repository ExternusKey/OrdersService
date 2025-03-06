using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ProcessingService.Config;

namespace ProcessingService.Kafka;

public class ProcessingServiceProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topicName;
    private readonly ILogger<ProcessingServiceProducer> _logger;

    public ProcessingServiceProducer(ILogger<ProcessingServiceProducer> logger,
        IOptions<ProducersConfig> producerConfig)
    {
        _logger = logger;
        _topicName = producerConfig.Value.TopicName;
        var config = new ProducerConfig
        {
            BootstrapServers = producerConfig.Value.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendStatusOrderAsync(string orderStatusJson)
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = orderStatusJson
        };

        await _producer.ProduceAsync(_topicName, message);
        _logger.LogInformation("Status order sending to Kafka");
    }
}