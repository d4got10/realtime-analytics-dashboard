using Microsoft.Extensions.Logging;

namespace DataAggregation.Application;

public class AggregationService : IAggregationService
{
    public AggregationService(ILogger<AggregationService> logger, IEventsStorage eventsStorage)
    {
        _logger = logger;
        _eventsStorage = eventsStorage;
    }

    private readonly ILogger<AggregationService> _logger;
    private readonly IEventsStorage _eventsStorage;

    public async Task AggregateEventAsync(string eventName)
    {
        _logger.LogInformation("Aggregated event \"{eventName}\".", eventName);
        await _eventsStorage.StoreAsync(eventName);
    }
}