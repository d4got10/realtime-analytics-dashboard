﻿using Authorization.Application.Hashing;
using Authorization.Application.Login;
using Authorization.Application.Registration;
using Authorization.Application.Repositories;
using Authorization.Application.Secrets;
using Authorization.Application.Time;
using Authorization.Application.Tokens;
using Authorization.Application.Tokens.Common;
using Authorization.Application.Tokens.Factories;
using Authorization.Application.Tokens.Services;
using Authorization.Application.Tokens.Validators;
using Authorization.Application.UnitsOfWork;
using Authorization.Infrastructure.Data;
using Authorization.Infrastructure.Repositories;
using Authorization.Infrastructure.Secrets;
using Authorization.Infrastructure.Time;
using Authorization.Infrastructure.UnitsOfWork;
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
        collection.AddScoped<ILoginService, LoginService>();
        collection.AddScoped<IClock, UtcSystemClock>();

        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();
        
        collection.AddTokenRelatedStuff(configuration);

        return collection;
    }

    private static void AddTokenRelatedStuff(this IServiceCollection collection, IConfiguration configuration)
    {
        string issuer = configuration["Tokens:Issuer"]!;
        string audience = configuration["Tokens:Audience"]!;
        int accessTokenDuration = int.Parse(configuration["Tokens:AccessTokenDuration"]!);
        int refreshTokenDuration = int.Parse(configuration["Tokens:RefreshTokenDuration"]!);

        collection.AddScoped<ITokenService, TokenService>();

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

        collection.AddScoped<ITokenPairFactory, TokenPairFactory>();
        
        collection.AddScoped<ITokenValidator>(services =>
        {
            var clock = services.GetRequiredService<IClock>();
            var secretStorage = services.GetRequiredService<ISecretStorage>();
            var settings = new JwtTokenSettings(TimeSpan.FromSeconds(refreshTokenDuration), issuer, audience);
            return new JwtTokenValidator(clock, secretStorage, settings);
        });
    }
}