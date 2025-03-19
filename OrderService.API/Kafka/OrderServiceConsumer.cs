using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.Config;
using OrderService.Services;
using ProcessingService.Models.Responses;

namespace OrderService.Kafka;

public class OrderServiceConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderServiceConsumer> _logger;

    public OrderServiceConsumer(IServiceProvider serviceProvider, ILogger<OrderServiceConsumer> logger,
        IOptions<ConsumersConfig> consumerConfig)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = consumerConfig.Value.BootstrapServers,
            GroupId = consumerConfig.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(consumerConfig.Value.TopicName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = await Task.Run(() => _consumer.Consume(stoppingToken), stoppingToken);
                var orderConfirmation =
                    JsonSerializer.Deserialize<OrderStatusConfirmationResponse>(consumeResult.Message.Value);

                if (orderConfirmation == null)
                    continue;
                _logger.LogInformation($"Order {orderConfirmation.OrderId} has been processed.");

                using var scope = _serviceProvider.CreateScope();
                var ordersService = scope.ServiceProvider.GetRequiredService<OrdersService>();

                await ordersService.OrderUpdateAsync(orderConfirmation.OrderId, orderConfirmation.OrderStatus,
                    orderConfirmation.RejectionReason);
            }
        }
        catch
        {
            _logger.LogError(" Error occured while processing message");
        }
        finally
        {
            _logger.LogInformation("Order service consumer stopped.");
            _consumer.Close();
        }
    }
}