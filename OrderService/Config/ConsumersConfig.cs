namespace OrderService.Config;

public static class ConsumersConfig
{
    public const string TopicName = "order_confirmation_status";
    public const string GroupId = "order_creators";
    public static readonly string? BootstrapServers;
    
    static ConsumersConfig()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        BootstrapServers = configuration["ConnectionStrings:BootstrapServices"];
    }
}