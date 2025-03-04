using System.Text.Json;
using Confluent.Kafka;
using ProcessingService.Config;
using Common.Models;
using ProcessingService.Models.Responses;
using ProcessingService.Services;

namespace ProcessingService.Kafka;

public class ProcessingServiceConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;
    public ProcessingServiceConsumer(IServiceProvider serviceProvider)
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
                    var orderRequest = JsonSerializer.Deserialize<Orders>(consumeResult.Message.Value);

                    //Новая модель под проверку доступности товара? [ProductId + Amount]
                    if (orderRequest == null) continue;
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
                Console.WriteLine($"[ProcessingService] Error occured while processing message - {ex}");
            }
            finally
            {
                _consumer.Close();
            }
        
    }
}