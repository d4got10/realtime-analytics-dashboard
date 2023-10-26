using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataAggregation.Infrastructure;

public class KafkaEventsConsumer : IEventsConsumer, IDisposable
{
    public KafkaEventsConsumer(IConfiguration configuration, ILogger<KafkaEventsConsumer> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig
        {
            GroupId = "events-consumer-group",
            BootstrapServers = configuration["Consumer:BootstrapServers"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe("events");
    }

    private readonly ILogger<KafkaEventsConsumer> _logger;
    private readonly IConsumer<Ignore, string> _consumer;

    private bool _isDisposed;

    public string? Consume()
    {
        try
        {
            ConsumeResult<Ignore, string>? consumeResult = _consumer.Consume();
            return consumeResult.Message.Value;
        }
        catch (ConsumeException e)
        {
            _logger.LogError("Error consuming message: {reason}", e.Error.Reason);
            return null;
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;
        _consumer.Dispose();
        GC.SuppressFinalize(this);
    }
}