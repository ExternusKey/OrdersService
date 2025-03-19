using System.Text.Json;
using Confluent.Kafka;
using ProcessingService.Config;
using Common.Models;
using Microsoft.Extensions.Options;
using ProcessingService.Models.Responses;
using ProcessingService.Services;

namespace ProcessingService.Kafka;

public class ProcessingServiceConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProcessingServiceConsumer> _logger;

    public ProcessingServiceConsumer(IServiceProvider serviceProvider, ILogger<ProcessingServiceConsumer> logger,
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
                var orderRequest = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

                if (orderRequest == null) continue;
                _logger.LogInformation($"Order: {orderRequest.Id} received.");

                using var scope = _serviceProvider.CreateScope();
                var productsService = scope.ServiceProvider.GetRequiredService<ProductsService>();
                var isProductAvailable =
                    await productsService.CheckProductAvailabilityAsync(orderRequest.ProductId);

                if (isProductAvailable.Status == ProductResponseStatus.ProductNotFound)
                {
                    await productsService.AddOrderAsync(orderRequest, false, isProductAvailable.Message);
                    continue;
                }

                var isAmountUpdated =
                    await productsService.UpdateProductAmountAsync(orderRequest.ProductId, orderRequest.Amount);

                if (isAmountUpdated.Status == ProductResponseStatus.NotEnoughAmount)
                {
                    await productsService.AddOrderAsync(orderRequest, false, isAmountUpdated.Message);
                    continue;
                }

                await productsService.AddOrderAsync(orderRequest, isAmountUpdated.IsSuccess);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured while processing message - {ex}");
        }
        finally
        {
            _logger.LogInformation("Processing service consumer stopped.");
            _consumer.Close();
        }
    }
}