using DataCollection.Application;
using Microsoft.Extensions.DependencyInjection;

namespace DataCollection.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IProducer, KafkaProducer>();
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IDataProcessor, DataProcessor>();
    }
}