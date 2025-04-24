using Microsoft.EntityFrameworkCore;
using PV260.Project.Domain;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Interfaces.Infrastructure.Persistence;
using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Mappers;

namespace PV260.Project.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        Models.UserEntity user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new NotFoundException(string.Format(Constants.Error.NotFoundFormat, nameof(User), nameof(email)));

        return user.ToDomainModel();
    }

    public async Task ToggleIsSubscribedAsync(string email)
    {
        _ = await _appDbContext.Users
            .Where(u => u.Email == email)
            .ExecuteUpdateAsync(u => u
                .SetProperty(s => s.IsSubscribed, s => !s.IsSubscribed)
            );
    }

    public async Task<IList<string>> GetSubscribedUserEmailsAsync()
    {
        return await _appDbContext.Users
            .Where(u => u.IsSubscribed)
            .Select(u => u.Email!)
            .ToListAsync();
    }
}
