using ProcessingService.Extensions;

var service = WebApplication.CreateBuilder(args);
var configuration = service.Configuration;
var services = service.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddProcessingService();
services.AddDataLayer(configuration);

var app = service.Build();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
