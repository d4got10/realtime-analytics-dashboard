using Authorization.Application.Repositories;

namespace Authorization.Application.UnitsOfWork;

public interface IUsersUnitOfWork
{
    public IUserRepository Users { get; }
    public Task SaveAsync(CancellationToken ct);
}