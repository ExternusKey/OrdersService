using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace OrderService.Kafka;

public class KafkaTopicInitializer : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {    
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    
        var bootstrapServers = configuration["ConnectionStrings:BootstrapServices"];
        
        try
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
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
                Console.WriteLine("[OrderService] Topic's created successfully.");
            }
        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"Error in creating topics: {e.Results[0].Error.Reason}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}