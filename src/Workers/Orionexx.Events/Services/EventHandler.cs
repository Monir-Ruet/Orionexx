using Microsoft.EntityFrameworkCore;
using Orionexx.Core.Shared.Enums.Events;
using Orionexx.Events.Data;

namespace Orionexx.Events.Services;

public interface IEventService
{
    Task ProcessBatchEvents(CancellationToken cancellationToken);
}

public class EventService(
    ApplicationDbContext dbContext,
    IEventPublisher eventPublisher) : IEventService
{
    private const int BatchSize = 100;
    public async Task ProcessBatchEvents(CancellationToken cancellationToken)
    {
        var events = await dbContext.Events
            .Where(m => m.Status == nameof(EventStatus.Pending))
            .OrderBy(m => m.CreatedAt)
            .Take(BatchSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        foreach (var ev in events)
        {
            ev.ProcessedAt = DateTime.UtcNow;
            ev.Status = nameof(EventStatus.Processing);
            
            await eventPublisher.PublishAsync(ev.MessageType, ev.Payload, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}