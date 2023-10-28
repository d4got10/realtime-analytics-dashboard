using Authorization.Application;
using Authorization.Infrastructure.Data;
using Authorization.Infrastructure.Repositories;
using Authorization.Infrastructure.Secrets;
using Authorization.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection collection, IConfiguration configuration)
    {
        return collection.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:Default"]));
    }

    public static IServiceCollection AddApplication(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddScoped<IUserRepository, UserRepository>();
        
        collection.AddScoped<IHashService, HashService>();
        collection.AddScoped<ISecretStorage, SecretsStorage>();
        collection.AddScoped<IRegistrationService, RegistrationService>();
        collection.AddScoped<IClock, UtcSystemClock>();

        collection.AddScoped<IUserRepository, UserRepository>();
        
        collection.AddTokenFactories(configuration);

        return collection;
    }

    private static void AddTokenFactories(this IServiceCollection collection, IConfiguration configuration)
    {
        string issuer = configuration["Tokens:Issuer"]!;
        string audience = configuration["Tokens:Audience"]!;
        int accessTokenDuration = int.Parse(configuration["Tokens:AccessTokenDuration"]!);
        int refreshTokenDuration = int.Parse(configuration["Tokens:RefreshTokenDuration"]!);

        collection.AddScoped<IAccessTokenFactory>(services =>
        {
            var clock = services.GetRequiredService<IClock>();
            var secretStorage = services.GetRequiredService<ISecretStorage>();
            var settings = new JwtTokenSettings(TimeSpan.FromSeconds(accessTokenDuration), issuer, audience);
            return new JwtTokenFactory(clock, secretStorage, settings);
        });

        collection.AddScoped<IRefreshTokenFactory>(services =>
        {
            var clock = services.GetRequiredService<IClock>();
            var secretStorage = services.GetRequiredService<ISecretStorage>();
            var settings = new JwtTokenSettings(TimeSpan.FromSeconds(refreshTokenDuration), issuer, audience);
            return new JwtTokenFactory(clock, secretStorage, settings);
        });
    }
}