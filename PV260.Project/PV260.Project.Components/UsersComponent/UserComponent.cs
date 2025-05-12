using PV260.Project.Components.UsersComponent.Services;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent;

public class UserComponent(IUserService userService) : IUserComponent
{
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await userService.GetUserByEmailAsync(email);
    }

    public async Task ToggleIsSubscribedAsync(string email)
    {
        await userService.ToggleIsSubscribedAsync(email);
    }

    public async Task<IList<string>> GetSubscribedEmailsAsync()
    {
        return await userService.GetSubscribedEmailsAsync();
    }
}