namespace Authorization.Application;

public interface IClock
{
    DateTime Now { get; }
}