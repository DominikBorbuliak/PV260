using Microsoft.EntityFrameworkCore;
using PV260.Project.BusinessLayer;
using PV260.Project.BusinessLayer.Exceptions;
using PV260.Project.BusinessLayer.Interfaces.DataAccessLayer;
using PV260.Project.BusinessLayer.Models;
using PV260.Project.DataAccessLayer.Mappers;
using PV260.Project.DataAccessLayer.Models;

namespace PV260.Project.DataAccessLayer.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        UserEntity user = await _appDbContext.Users
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
}
