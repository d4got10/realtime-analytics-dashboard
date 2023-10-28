using DataAggregation.Application;
using DataAggregation.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAggregation.Infrastructure.Repositories;

public class PostgresEventsRepository : IEventsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostgresEventsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(string eventName)
    {
        var @event = new Event
        {
            Name = eventName
        };
        await _dbContext.Events.AddAsync(@event);
        await _dbContext.SaveChangesAsync();
    }

    public Task<IEnumerable<Event>> GetPageNoTrackingAsync(int pageIndex, int countPerPage)
    {
        IQueryable<Event> result = _dbContext.Events.Skip(pageIndex * countPerPage).Take(countPerPage).AsNoTracking();
        return Task.FromResult(result.AsEnumerable());
    }
}