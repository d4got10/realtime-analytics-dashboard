using DataAggregation.Domain;

namespace DataAggregation.Application;

public interface IEventsRepository
{
    Task AddAsync(string eventName);
    Task<IEnumerable<Event>> GetPageNoTrackingAsync(int pageIndex, int countPerPage);
}