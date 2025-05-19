using OrderService.Extensions;

var service = WebApplication.CreateBuilder(args);
var configuration = service.Configuration;
var services = service.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHealthChecks();
services.AddControllers();

services.AddKafkaConfigs(configuration)
    .AddOrderService()
    .AddDataLayer(configuration);

var app = service.Build();

app.MapHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI();

app.Services.InitializeDatabase();
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}