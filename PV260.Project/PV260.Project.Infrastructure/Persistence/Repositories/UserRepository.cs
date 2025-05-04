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
        var result = await (
            from u in _appDbContext.Users
            join ur in _appDbContext.UserRoles on u.Id equals ur.UserId
            join r in _appDbContext.Roles on ur.RoleId equals r.Id
            where u.Email == email
            select new 
            {
                User = u,
                RoleName = r.Name!
            }
        ).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new NotFoundException(
                string.Format(Constants.Error.NotFoundFormat, nameof(User), nameof(email))
            );
        }

        return result.User.ToDomainModel(result.RoleName);
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
