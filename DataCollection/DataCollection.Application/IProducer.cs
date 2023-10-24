namespace DataCollection.Application;

public interface IProducer
{
    Task SendAsync(string topic, string message);
}