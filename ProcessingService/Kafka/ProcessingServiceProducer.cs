using Confluent.Kafka;
using ProcessingService.Config;
using ProcessingService.Models.Responses;

namespace ProcessingService.Kafka;

public class ProcessingServiceProducer
{
    private readonly IProducer<string, string> _producer;
    private const string TopicName = ProducersConfig.TopicName;

    public ProcessingServiceProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = ProducersConfig.BootstrapServers,
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
        
        await _producer.ProduceAsync(TopicName, message);
        Console.WriteLine("[Processing] Order status send to Kafka...");
    }

}