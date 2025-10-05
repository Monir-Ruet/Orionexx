using Orionexx.Core.Shared.Entities.Events;

namespace Orionexx.Identity.Application.Interfaces.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetEventsBatchAsync(int batchSize, CancellationToken cancellationToken);
    Task AddEventAsync(Event ev, CancellationToken cancellationToken = default);
    Task UpdateEventsAsync(List<Event> events, CancellationToken cancellationToken = default);
}
