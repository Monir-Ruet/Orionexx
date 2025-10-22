using Orionexx.Messaging.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureWebApplication();

app.MapGet("/", () => "Hello Orionexx Messaging!");

app.Run();