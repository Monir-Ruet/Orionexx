using CommunityToolkit.Aspire.Hosting.Dapr;

namespace Orionexx.AppHost.Extensions;

public static class DistributedApplicationBuilderExtension
{
    public static void ConfigureDistributedApplication(this IDistributedApplicationBuilder builder)
    {
        var pubsubComponent = builder.AddDaprPubSub("pubsub");

        builder.AddProject<Projects.Orionexx_Identity_Grpc>("OrionexxIdentity")
            .WithReference(pubsubComponent)
            .WithDaprSidecar(new DaprSidecarOptions
            {
                AppId = "OrionexxIdentity",
                AppPort = 5098,
                AppProtocol = "grpc"
            });

        builder.AddProject<Projects.Orionexx_Web>("Orionexx")
            .WithReference(pubsubComponent)
            .WithDaprSidecar(new DaprSidecarOptions
            {
                AppId = "Orionexx",
                AppProtocol = "grpc"
            });
    }
}