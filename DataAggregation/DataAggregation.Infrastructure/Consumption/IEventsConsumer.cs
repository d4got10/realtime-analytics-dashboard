namespace DataAggregation.Infrastructure;

public interface IEventsConsumer
{
    string? Consume();
}