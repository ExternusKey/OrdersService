using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OrderService.Kafka;
using OrderService.Services;
using OrderService.Services.Interfaces;

namespace OrderService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        services.AddScoped<OrderServiceProducer>();
        services.AddScoped<OrdersService>();
        services.AddScoped<ProductsService>();
        services.AddScoped<OrderDbContext>();
        services.AddHostedService<OrderServiceConsumer>();

        return services;
    }

    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {        
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddTransient<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString));

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