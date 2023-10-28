using DataAggregation.Application;
using DataAggregation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAggregation.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IAggregationService, AggregationService>();
    }

    public static IServiceCollection AddEventsConsumer(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IEventsConsumer, KafkaEventsConsumer>();
    }

    public static IServiceCollection AddEventsStorage(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IEventsRepository, PostgresEventsRepository>();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration["ConnectionStrings:Default"]);
        });
    }
}