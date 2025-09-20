using Orionexx.Events.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServices();

var host = builder.Build();
host.Run();