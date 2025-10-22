using Microsoft.EntityFrameworkCore;
using Orionexx.Core.Shared.Entities.Events;
using Orionexx.Core.Shared.Enums.Events;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Infrastructure.Persistence;

namespace Orionexx.Identity.Infrastructure.Repositories;

public class EventRepository(ApplicationDbContext dbContext) : IEventRepository
{
    public async Task<IEnumerable<Event>> GetEventsBatchAsync(int batchSize, CancellationToken cancellationToken)
    {
        var events = await dbContext.Events
            .Where(m => m.Status == EventStatus.Pending.ToString())
            .OrderBy(m => m.CreatedAt)
            .Take(batchSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return events;
    }

    public async Task AddEventAsync(Event ev, CancellationToken cancellationToken = default)
    {
        await dbContext.Events.AddAsync(ev, cancellationToken);
    }

    public async Task UpdateEventsAsync(List<Event> events, CancellationToken cancellationToken = default)
    {
        await dbContext.Events.BulkUpdateAsync(events);
    }
}
