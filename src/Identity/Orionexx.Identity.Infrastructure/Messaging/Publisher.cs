using System.Text.Json;
using Dapr.Client;
using Orionexx.Core.Shared.Constants;

namespace Orionexx.Identity.Infrastructure.Messaging;

public interface IPublisher
{
    Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken = default);
}

public class Publisher(DaprClient dapr) : IPublisher
{
    public async Task PublishAsync(string eventType, string payload, CancellationToken cancellationToken = default)
    {
        var payloadElement = JsonSerializer.Deserialize<JsonElement>(payload);
        await dapr.PublishEventAsync(TopicConstants.PubSub, eventType, payloadElement, cancellationToken);
    }
}
