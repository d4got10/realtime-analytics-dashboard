namespace DataCollection.Application;

public class DataProcessor : IDataProcessor
{
    public DataProcessor(IProducer producer)
    {
        _producer = producer;
    }

    private readonly IProducer _producer;

    public async Task ProcessEventAsync(string eventName)
    {
        await ProduceEventAsync(eventName);
    }

    private async Task ProduceEventAsync(string eventName)
    {
        await _producer.SendAsync("events", eventName);
    }
}