using DataAggregation.Application;
using Microsoft.Extensions.DependencyInjection;

namespace DataAggregation.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IAggregationService, AggregationService>();
    }

    public static IServiceCollection AddKafkaEventsConsumer(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IEventsConsumer, KafkaEventsConsumer>();
    }
}