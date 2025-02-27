using System.Text.Json;
using Confluent.Kafka;
using OrderService.Config;
using OrderService.Models;

namespace OrderService.Kafka;

public class OrderServiceConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    public OrderServiceConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = ConsumersConfig.BootstrapServers,
            GroupId = ConsumersConfig.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(ConsumersConfig.TopicName);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = await Task.Run(() => _consumer.Consume(stoppingToken), stoppingToken);
            var orderConfirmation = JsonSerializer.Deserialize<OrderConfirmation>(consumeResult.Message.Value);
            if (orderConfirmation?.OrderStatus != "confirmed")
            {
                // TODO: Ордер отклонен
            }
            //TODO: Ордер разрешен и нужно прописать логику удаления товаров из таблицы + положительный ответ
        }
    }
}
