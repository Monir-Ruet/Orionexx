using System.Text.Json;
using Dapr.Client;

namespace Orionexx.Identity.Infrastructure.Messaging;

public interface IPublisher
{
    Task PublishAsync<T>(string eventType, T payload, CancellationToken cancellationToken = default);
}

public class Publisher(DaprClient dapr) : IPublisher
{
    private const string PubSubName = "pubsub";
    public async Task PublishAsync<T>(string eventType, T payload, CancellationToken cancellationToken = default)
    {
        await dapr.PublishEventAsync(PubSubName, eventType, payload, cancellationToken);
    }
}
