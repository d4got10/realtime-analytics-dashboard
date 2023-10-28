using Microsoft.Extensions.Logging;

namespace DataAggregation.Application;

public class AggregationService : IAggregationService
{
    public AggregationService(ILogger<AggregationService> logger, IEventsRepository eventsRepository)
    {
        _logger = logger;
        _eventsRepository = eventsRepository;
    }

    private readonly ILogger<AggregationService> _logger;
    private readonly IEventsRepository _eventsRepository;

    public async Task AggregateEventAsync(string eventName)
    {
        _logger.LogInformation("Aggregated event \"{eventName}\".", eventName);
        await _eventsRepository.AddAsync(eventName);
    }
}