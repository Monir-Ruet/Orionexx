using System.Text.Json;
using Dapr.Client;
using Orionexx.Core.Shared.Constants;

namespace Orionexx.Identity.Infrastructure.Messaging;

public interface IPublisher
{
    Task PublishAsync<T>(string eventType, T payload, CancellationToken cancellationToken = default);
}

public class Publisher(DaprClient dapr) : IPublisher
{
    public async Task PublishAsync<T>(string eventType, T payload, CancellationToken cancellationToken = default)
    {
        await dapr.PublishEventAsync(TopicConstants.PubSub, eventType, payload, cancellationToken);
    }
}
