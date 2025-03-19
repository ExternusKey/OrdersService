using System.Data.Common;
using Common.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderService.Kafka;
using OrderService.Kafka.Interfaces;

namespace IntegrationalTests;

public class ServiceWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private static Mock<IOrderServiceProducer> _orderServiceProducerMoq;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            services.AddControllers();
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<OrderDbContext>));
            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);

            services.AddDbContext<OrderDbContext>(options => options.UseInMemoryDatabase(databaseName: "TestDatabase"));

            var kafkaProducerDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IOrderServiceProducer));
            services.Remove(kafkaProducerDescriptor);

            _orderServiceProducerMoq = new Mock<IOrderServiceProducer>();
            services.AddScoped<IOrderServiceProducer>(_ => _orderServiceProducerMoq.Object);

            var kafkaInitializerDescriptor = services.SingleOrDefault(
                d => d.ImplementationType ==
                     typeof(KafkaInitializer));
            services.Remove(kafkaInitializerDescriptor);

            var kafkaConsumerDescriptor = services.SingleOrDefault(
                d => d.ImplementationType ==
                     typeof(OrderServiceConsumer));
            services.Remove(kafkaConsumerDescriptor);
        });
    }
}