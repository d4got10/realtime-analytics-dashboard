using Authorization.Application.Repositories;
using Authorization.Domain;
using Authorization.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private readonly ApplicationDbContext _dbContext;

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken ct)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username, ct);
    }
    
    public async Task<User?> FindByUsernameAsyncNoTracking(string username, CancellationToken ct)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Username == username, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _dbContext.AddAsync(user, ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}