using Authorization.Application.Repositories;
using Authorization.Application.UnitsOfWork;
using Authorization.Infrastructure.Data;

namespace Authorization.Infrastructure.UnitsOfWork;

public class UsersUnitOfWork : IUsersUnitOfWork
{
    public UsersUnitOfWork(ApplicationDbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        Users = userRepository;
    }

    public IUserRepository Users { get; }
    
    private readonly ApplicationDbContext _dbContext;

    public async Task SaveAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}