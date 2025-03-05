using System.Text.Json;
using Common.Models;
using Confluent.Kafka;
using OrderService.Config;
using OrderService.Models;
using OrderService.Services;
using ProcessingService.Models.Responses;

namespace OrderService.Kafka;

public class OrderServiceConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;
    
    public OrderServiceConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
      
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = await Task.Run(() => _consumer.Consume(stoppingToken), stoppingToken);
                    var orderConfirmation = JsonSerializer.Deserialize<OrderStatusConfirmationResponse>(consumeResult.Message.Value);
                    
                    if (orderConfirmation == null)
                        continue;
                    Console.WriteLine($"[OrderService] Order: {orderConfirmation.OrderId} received.");
                    using var scope = _serviceProvider.CreateScope();
                    var ordersService = scope.ServiceProvider.GetRequiredService<OrdersService>();
                    
                    await ordersService.OrderUpdateAsync(orderConfirmation.OrderId, orderConfirmation.OrderStatus, orderConfirmation.RejectionReason);
                }
            }
            catch
            {
                Console.WriteLine("[OrderService] Error occured while processing message");
            }
            finally
            {
                _consumer.Close();
            }
        
    }
}
