using Orionexx.Messaging.Dtos;
using Orionexx.Messaging.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseCloudEvents();
app.MapSubscribeHandler();

// app.MapPost("AccountCreated", (AccountCreated data) =>
// {
//     Console.WriteLine($"AccountCreated event received: {data.Email}");
//     return Results.Ok();
// })
// .WithTopic("pubsub", "AccountCreated");

app.Run();
