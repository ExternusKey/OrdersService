using Common.Models;
using Confluent.Kafka;

namespace ProcessingService.Kafka.Interfaces;

public interface IProcessingServiceProducer
{
    Task SendStatusOrderAsync(string orderStatusJson);
}