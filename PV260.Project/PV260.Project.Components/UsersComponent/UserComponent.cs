using Microsoft.AspNetCore.Identity;
using PV260.Project.Components.UsersComponent.Services;
using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Components.UsersComponent;

public class UserComponent : IUserComponent
{
    private readonly IUserService _userService;
    private readonly SignInManager<UserEntity> _signInManager;

    public UserComponent(IUserService userService, SignInManager<UserEntity> signInManager)
    {
        _userService = userService;
        _signInManager = signInManager;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
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