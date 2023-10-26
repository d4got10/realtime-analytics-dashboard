using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataAggregation.Infrastructure;

public static class HostExtensions
{
    public static IHost ApplyMigrations(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        
        IServiceProvider services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        return host;
    }
}