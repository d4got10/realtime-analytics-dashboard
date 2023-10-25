namespace DataAggregation.Application;

public interface IAggregationService
{
    void AggregateEvent(string eventName);
}