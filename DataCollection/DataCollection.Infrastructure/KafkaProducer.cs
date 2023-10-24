using DataCollection.Application;
using Microsoft.Extensions.Logging;

namespace DataCollection.Infrastructure;

public class KafkaProducer : IProducer
{
    public KafkaProducer(ILogger<KafkaProducer> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<KafkaProducer> _logger;

    public Task SendAsync(string topic, string message)
    {
        _logger.LogInformation("Sent \"{message}\" to topic \"{topic}\"", message, topic);
        return Task.CompletedTask;
    }
}