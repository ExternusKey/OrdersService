namespace ProcessingService.Config;

public class ConsumersConfig
{
    public string TopicName { get; set; }
    public string GroupId { get; set; }
    public string BootstrapServers { get; set; }
}