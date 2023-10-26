namespace DataAggregation.Application;

public interface IEventsStorage
{
    Task StoreAsync(string eventName);
}