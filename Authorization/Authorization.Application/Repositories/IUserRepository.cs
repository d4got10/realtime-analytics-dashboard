using Authorization.Domain;

namespace Authorization.Application;

public interface IUserRepository
{
    Task Add(User user);
}