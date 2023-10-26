namespace DataAggregation.Application;

public interface IAggregationService
{
    Task AggregateEventAsync(string eventName);
}