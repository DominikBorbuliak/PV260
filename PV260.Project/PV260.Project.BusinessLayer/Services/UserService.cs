using PV260.Project.BusinessLayer.Interfaces.BusinessLayer;
using PV260.Project.BusinessLayer.Interfaces.DataAccessLayer;
using PV260.Project.BusinessLayer.Models;

namespace PV260.Project.BusinessLayer.Services;

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
