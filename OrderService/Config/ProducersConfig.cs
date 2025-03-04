﻿namespace OrderService.Config;

public static class ProducersConfig
{
    public const string TopicName = "order_created";
    public static readonly string? BootstrapServers;
    
    static ProducersConfig()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        BootstrapServers = configuration["ConnectionStrings:BootstrapServices"];
    }
}