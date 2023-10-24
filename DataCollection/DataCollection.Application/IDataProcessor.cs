namespace DataCollection.Application;

public interface IDataProcessor
{
    Task ProcessEventAsync(string eventName);
}