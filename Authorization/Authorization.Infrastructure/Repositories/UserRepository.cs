using Authorization.Application;
using Authorization.Domain;
using Authorization.Infrastructure.Data;

namespace Authorization.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private readonly ApplicationDbContext _dbContext;

    public async Task Add(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}