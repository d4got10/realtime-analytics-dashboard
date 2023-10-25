using Confluent.Kafka;
using DataCollection.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataCollection.Infrastructure;

public class KafkaProducer : IProducer, IDisposable
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<Null, string> _kafkaProducer;

    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Producer:BootstrapServers"]
        };

        _kafkaProducer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendAsync(string topic, string message)
    {
        try
        {
            DeliveryResult<Null, string>? deliveryReport = await _kafkaProducer.ProduceAsync(topic, new Message<Null, string>
            {
                Key = null!,
                Value = message
            });

            _logger.LogInformation("Sent message to Kafka topic \"{topic}\" with offset: {offset}", deliveryReport.Topic, deliveryReport.Offset);
        }
        catch (ProduceException<string, string> e)
        {
            _logger.LogError("Error while sending Kafka: {reason}", e.Error.Reason);
        }
    }

    public void Dispose()
    {
        _kafkaProducer.Dispose();
        GC.SuppressFinalize(this);
    }
}