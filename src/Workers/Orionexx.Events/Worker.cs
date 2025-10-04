using Orionexx.Events.Logging;
using Orionexx.Events.Services;

namespace Orionexx.Events;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory serviceFactory) : IHostedService, IDisposable
{
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.StartHostedService();
        _timer = new Timer(async _ => await OnTimerFiredAsync(cancellationToken), null, NextTimerFireMilliseconds(), Timeout.Infinite);
        return Task.CompletedTask;
    }

    private async Task OnTimerFiredAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceFactory.CreateScope();
            var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
            await eventService.ProcessBatchEvents(cancellationToken);
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

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        logger.StopHostedService();
        return Task.CompletedTask;
    }

    private int NextTimerFireMilliseconds()
    {
        var ts = TimeSpan.FromSeconds(2);
        return (int)ts.TotalMilliseconds;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}