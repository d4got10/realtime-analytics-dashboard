using Microsoft.Extensions.Configuration;

namespace DataAggregation.Infrastructure;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddInfrastructure(this ConfigurationManager configurationManager)
    {
        return configurationManager.AddJsonFile("appsettings.json");
    }
}