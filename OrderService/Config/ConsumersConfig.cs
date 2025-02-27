using Confluent.Kafka;

namespace OrderService.Config;

public class ConsumersConfig
{
    public const string TopicName = "order_confirmation";
    public const string BootstrapServers = "localhost:29092";
    public const string GroupId = "order_creators";
}