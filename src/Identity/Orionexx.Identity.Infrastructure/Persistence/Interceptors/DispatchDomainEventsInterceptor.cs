using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Orionexx.Core.Shared.Entities.Events;
using Orionexx.Core.Shared.Primitives;

namespace Orionexx.Identity.Infrastructure.Persistence.Interceptors;

public class DispatchDomainEventsInterceptor(ApplicationDbContext dbContext) : SaveChangesInterceptor
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents((ApplicationDbContext?)eventData.Context).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents((ApplicationDbContext?)eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEvents(ApplicationDbContext? context)
    {
        if (context == null) return;

        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            var outboxMessage = new Event
            {
                EventType = domainEvent.GetType().FullName ?? string.Empty,
                Payload = JsonSerializer.Serialize(domainEvent) ?? string.Empty,
                Destination = "IdentityService"
            };
            await dbContext.Events.AddAsync(outboxMessage);
        }
    }
}
