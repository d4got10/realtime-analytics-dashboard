using DataAggregation.Application;
using DataAggregation.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataAggregation.CLI;

public sealed class EventMessagesConsumeWorker : IHostedService
{
    public EventMessagesConsumeWorker(IEventsConsumer eventsConsumer, IAggregationService messageWriter, 
        ILogger<EventMessagesConsumeWorker> logger)
    {
        _eventsConsumer = eventsConsumer;
        _aggregationService = messageWriter;
        _logger = logger;
    }

    private readonly IEventsConsumer _eventsConsumer;
    private readonly IAggregationService _aggregationService;
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

                await _aggregationService.AggregateEventAsync(eventName);
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