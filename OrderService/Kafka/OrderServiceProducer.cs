﻿using Confluent.Kafka;
using OrderService.Config;
using OrderService.Services;

namespace OrderService.Kafka;

public class OrderServiceProducer
{
    private readonly IProducer<string, string> _producer;
    private const string TopicName = ProducersConfig.TopicName;
    public OrderServiceProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = ProducersConfig.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendOrderInfoAsync(string orderRequestJson)
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = orderRequestJson
        };
        await _producer.ProduceAsync(TopicName, message);
        Console.WriteLine("[OrderService] Order request send to Kafka...");
    }
}