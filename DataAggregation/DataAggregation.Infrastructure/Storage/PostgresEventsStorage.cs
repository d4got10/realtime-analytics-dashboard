using DataAggregation.Application;
using DataAggregation.Domain;

namespace DataAggregation.Infrastructure;

public class PostgresEventsStorage : IEventsStorage
{
    private readonly ApplicationDbContext _dbContext;

    public PostgresEventsStorage(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task StoreAsync(string eventName)
    {
        var @event = new Event
        {
            Name = eventName
        };
        await _dbContext.Events.AddAsync(@event);
        await _dbContext.SaveChangesAsync();
    }
}