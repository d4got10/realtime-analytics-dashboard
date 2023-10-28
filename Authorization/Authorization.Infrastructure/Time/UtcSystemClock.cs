using Authorization.Application;

namespace Authorization.Infrastructure.Time;

public class UtcSystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}