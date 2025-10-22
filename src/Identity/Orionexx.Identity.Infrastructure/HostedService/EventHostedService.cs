using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orionexx.Core.Shared.Entities.Events;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Core.Logging;
using Orionexx.Identity.Infrastructure.Messaging;

namespace Orionexx.Identity.Infrastructure.HostedService;

public class EventHostedService(
    ILogger<EventHostedService> logger,
    IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private Timer? _timer;
    private const int BatchSize = 100;
    private const int MaxRetryCount = 5;
    private const int NextTimerFireSeconds = 10;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.StartHostedService();
        _timer = new Timer(async _ => await OnTimerFiredAsync(cancellationToken), null, NextTimerFireMilliseconds(), Timeout.Infinite);
        return Task.CompletedTask;
    }

    private async Task OnTimerFiredAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;
        try
        {
            using var scope = serviceProvider.CreateScope();
            var eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
            var events = await eventRepository.GetEventsBatchAsync(BatchSize, cancellationToken);
            if (!events.Any())
            {
                logger.NoEventsToProcess();
                return;
            }
            await ProcessEventsBatchAsync([.. events], scope.ServiceProvider.GetRequiredService<IPublisher>(), cancellationToken);
            await eventRepository.UpdateEventsAsync([.. events], cancellationToken);
        }
        catch (Exception ex)
        {
            logger.BatchProcessingError(ex);
        }
        finally
        {
            _timer?.Change(NextTimerFireMilliseconds(), Timeout.Infinite);
        }
    }

    private async Task<List<Event>> ProcessEventsBatchAsync(
        List<Event> events,
        IPublisher publisher,
        CancellationToken cancellationToken)
    {
        var processedEvents = new List<Event>();
        var failedEvents = new List<Event>();

        foreach (var ev in events)
        {
            try
            {
                await publisher.PublishAsync(ev.EventType, ev.Payload, cancellationToken);
                ev.MarkAsProcessed();
                processedEvents.Add(ev);

                logger.EventPublishedSuccessfully(ev.Id, ev.EventType);
            }
            catch (Exception ex)
            {
                logger.EventPublishFailed(ev.Id, ev.EventType, ex);
                ev.MarkForRetry();
                failedEvents.Add(ev);

                if (ev.RetryCount >= MaxRetryCount)
                {
                    ev.MarkAsFailed(ex.Message);
                    logger.EventMaxRetriesExceeded(ev.Id);
                }
            }
        }

        if (failedEvents.Count != 0)
        {
            processedEvents.AddRange(failedEvents);
        }

        return processedEvents;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        logger.StopHostedService();
        return Task.CompletedTask;
    }

    private int NextTimerFireMilliseconds()
    {
        var ts = TimeSpan.FromSeconds(NextTimerFireSeconds);
        return (int)ts.TotalMilliseconds;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}