using DataAggregation.Application;
using DataAggregation.Infrastructure;

namespace DataAggregation.API.Workers;

public sealed class EventMessagesConsumeWorker : IHostedService
{
    public EventMessagesConsumeWorker(IEventsConsumer eventsConsumer, IServiceScopeFactory serviceScopeFactory, 
        ILogger<EventMessagesConsumeWorker> logger)
    {
        _eventsConsumer = eventsConsumer;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    private readonly IEventsConsumer _eventsConsumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EventMessagesConsumeWorker> _logger;

    private readonly CancellationTokenSource _cts = new();

    public Task StartAsync(CancellationToken cancellationToken)
    { 
        Task.Run(() => ExecuteAsync(_cts.Token), _cts.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Waiting for event message...");
                string? eventName = _eventsConsumer.Consume();
                if (eventName == null) continue;

                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    var aggregationService = scope.ServiceProvider.GetRequiredService<IAggregationService>();
                    await aggregationService.AggregateEventAsync(eventName);
                }
                await Task.Yield();
            }
        }
        catch (Exception exception)
        {
            _logger.LogError("Exception occured while consuming events: {exception}", exception);
            throw;
        }
    }
}