using Orionexx.Identity.Grpc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureWebApplicationServices();

var app = builder.Build();

app.ConfigureWebApplication();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();