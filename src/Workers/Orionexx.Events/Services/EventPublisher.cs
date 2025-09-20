using Dapr.Client;

namespace Orionexx.Events.Services;

public interface IEventPublisher
{
    Task PublishAsync(string messageType, string payload, CancellationToken cancellationToken);
}

public class EventPublisher(DaprClient daprClient) : IEventPublisher
{
    private const string PubSubName = "pubsub";

    public async Task PublishAsync(string messageType, string payload, CancellationToken cancellationToken)
    {
        await daprClient.PublishEventAsync(PubSubName, messageType, payload, cancellationToken);
    }
}