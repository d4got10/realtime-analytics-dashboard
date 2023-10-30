namespace Authorization.Application.Time;

public interface IClock
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}