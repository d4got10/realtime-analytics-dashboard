using Microsoft.Extensions.Logging;

namespace DataAggregation.Application;

public class AggregationService : IAggregationService
{
    public AggregationService(ILogger<AggregationService> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<AggregationService> _logger;

    public void AggregateEvent(string eventName)
    {
        _logger.LogInformation("Aggregated event \"{eventName}\".", eventName);
    }
}