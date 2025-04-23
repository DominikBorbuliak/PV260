using PV260.Project.Domain.Interfaces.Domain;
using PV260.Project.Domain.Interfaces.Infrastructure.Persistence;
using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task ToggleIsSubscribedAsync(string email)
    {
        await _userRepository.ToggleIsSubscribedAsync(email);
    }
}
