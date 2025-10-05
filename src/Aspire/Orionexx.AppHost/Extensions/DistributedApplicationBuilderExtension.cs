using CommunityToolkit.Aspire.Hosting.Dapr;

namespace Orionexx.AppHost.Extensions;

public static class DistributedApplicationBuilderExtension
{
    public static void ConfigureDistributedApplication(this IDistributedApplicationBuilder builder)
    {
        var sqlServerConnectionString = builder.AddConnectionString("Orionexx");
        
        var pubSub = builder.AddDaprPubSub("pubsub", new DaprComponentOptions()
        {
            LocalPath = "../../../dapr/components/pubsub.rabbitmq.yaml"
        });
        
        builder.AddProject<Projects.Orionexx_Identity_Grpc>("OrionexxIdentity")
            .WithReference(sqlServerConnectionString)
            .WithReference(pubSub)
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

        builder.AddProject<Projects.Orionexx_Messaging>("OrionexxMessaging")
            .WithReference(sqlServerConnectionString)
            .WithReference(pubSub)
            .WithDaprSidecar(new DaprSidecarOptions
            {
                AppId = "OrionexxMessagingSubscriber",
                AppPort = 5130,
                AppProtocol = "http"
            });
    }
}