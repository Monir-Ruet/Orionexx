using Orionexx.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

builder.ConfigureDistributedApplication();

builder.Build().Run();
