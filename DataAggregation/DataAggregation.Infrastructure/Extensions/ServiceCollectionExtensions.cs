using DataAggregation.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAggregation.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IAggregationService, AggregationService>();
    }

    public static IServiceCollection AddEventsConsumer(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IEventsConsumer, KafkaEventsConsumer>();
    }

    public static IServiceCollection AddEventsStorage(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IEventsStorage, PostgresEventsStorage>();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration["ConnectionStrings:Default"]);
        });
    }
}