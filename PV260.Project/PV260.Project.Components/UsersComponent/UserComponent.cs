using PV260.Project.Components.UsersComponent.Services;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent;

public class UserComponent : IUserComponent
{
    private readonly UserService _userService;

    public UserComponent(UserService userService)
    {
        _userService = userService;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userService.GetUserByEmailAsync(email);
    }

    public async Task ToggleIsSubscribedAsync(string email)
    {
        await _userService.ToggleIsSubscribedAsync(email);
    }

    public async Task<IList<string>> GetSubscribedEmailsAsync()
    {
        return await _userService.GetSubscribedEmailsAsync();
    }
}