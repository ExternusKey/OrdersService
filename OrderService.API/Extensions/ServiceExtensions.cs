using Common.Repositories;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.EntityFrameworkCore;
using OrderService.Config;
using OrderService.Kafka;
using OrderService.Kafka.Interfaces;
using OrderService.Services;

namespace OrderService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        services.AddSingleton<IOrderServiceProducer, OrderServiceProducer>();
        services.AddScoped<OrdersService>();
        services.AddScoped<ProductsService>();
        services.AddHostedService<OrderServiceConsumer>();
        //Manual creating topics in Kafka
        //services.AddHostedService<KafkaInitializer>();
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

    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        dbContext.Database.EnsureCreated();

        DataSeeder.SeedGpuData(dbContext);
    }
}