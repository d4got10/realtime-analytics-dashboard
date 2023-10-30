using Authorization.Domain;

namespace Authorization.Application.Repositories;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken ct);
    Task<User?> FindByUsernameAsync(string username, CancellationToken ct);
    Task<User?> FindByUsernameAsyncNoTracking(string username, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}