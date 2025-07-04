using CommunityToolkit.Aspire.Hosting.Dapr;

namespace Orionexx.AppHost.Extensions;

public static class DistributedApplicationBuilderExtension
{
    public static void ConfigureDistributedApplication(this IDistributedApplicationBuilder builder)
    {
        var sqlServerConnectionString = builder.AddConnectionString("Orionexx");

        builder.AddProject<Projects.Orionexx_Identity_Grpc>("OrionexxIdentity")
            .WithReference(sqlServerConnectionString)
            .WithDaprSidecar(new DaprSidecarOptions
            {
                AppId = "OrionexxIdentity",
                AppPort = 5098,
                AppProtocol = "grpc"
            });

        builder.AddProject<Projects.Orionexx_Web>("OrionexxWeb")
            .WithDaprSidecar(new DaprSidecarOptions
            {
                AppId = "Orionexx",
                AppProtocol = "grpc"
            });
    }
}