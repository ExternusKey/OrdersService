using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProcessingService.Kafka;
using ProcessingService.Services;
using ProcessingService.Services.Interfaces;

namespace ProcessingService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddProcessingService(this IServiceCollection services)
    {
        services.AddScoped<ProductsService>();
        services.AddScoped<OrderDbContext>();
        services.AddScoped<ProcessingServiceProducer>();
        services.AddHostedService<ProcessingServiceConsumer>();
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
    
}