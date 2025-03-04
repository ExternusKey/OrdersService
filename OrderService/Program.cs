using System.Text.Json;
using System.Text.Json.Serialization;

using OrderService.Extensions;

var service = WebApplication.CreateBuilder(args);
var configuration = service.Configuration;

service.Services.AddEndpointsApiExplorer();
service.Services.AddSwaggerGen();

service.Services.AddOrderService();
service.Services.AddDataLayer(configuration);

service.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
service.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.WriteIndented = true;
});

var app = service.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.Services.InitializeDatabase();
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();