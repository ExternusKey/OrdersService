using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace OrderService.Kafka;

public class KafkaInitializer(IConfiguration configuration, ILogger<KafkaInitializer> logger) : IHostedService
{
    private readonly string? _bootstrapServers = configuration["ConnectionStrings:BootstrapServers"];

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var adminClient =
                new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build();
            var topicNames = new[] { "order_confirmation_status", "order_created" };

            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
            var existingTopics = metadata.Topics.Select(t => t.Topic).ToHashSet();

            var topicsToCreate = topicNames
                .Where(t => !existingTopics.Contains(t))
                .Select(t => new TopicSpecification { Name = t, NumPartitions = 2, ReplicationFactor = 1 })
                .ToList();

            if (topicsToCreate.Count > 0)
            {
                await adminClient.CreateTopicsAsync(topicsToCreate);
                logger.LogInformation("Created topics");
            }
        }
        catch (CreateTopicsException e)
        {
            logger.LogError($"Error in creating topics: {e.Results[0].Error.Reason}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}