using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using ProcessingService.Config;
using ProcessingService.Kafka;
using ProcessingService.Kafka.Interfaces;
using ProcessingService.Services;

namespace ProcessingService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddProcessingService(this IServiceCollection services)
    {
        services.AddScoped<ProductsService>();
        services.AddScoped<IProcessingServiceProducer, ProcessingServiceProducer>();
        services.AddHostedService<ProcessingServiceConsumer>();

        return services;
    }

    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddKafkaConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProducersConfig>(configuration.GetSection("ProducersConfig"));
        services.Configure<ConsumersConfig>(configuration.GetSection("ConsumersConfig"));

        return services;
    }
}