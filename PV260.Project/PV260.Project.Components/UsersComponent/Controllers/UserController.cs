using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Components.Common.Controllers;
using PV260.Project.Components.UsersComponent.Dtos;
using PV260.Project.Components.UsersComponent.Extensions;
using PV260.Project.Components.UsersComponent.Mappers;
using PV260.Project.Components.UsersComponent.Services;
using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Components.UsersComponent.Controllers;

[Authorize]
public class UserController : ApiController
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IUserService _userService;
    public UserController(SignInManager<UserEntity> signInManager, IUserService userService)
    {
        _signInManager = signInManager;
        _userService = userService;
    }

    [HttpPost("logout", Name = "logout")]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok();
    }

    [HttpGet("me", Name = "getMe")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        string email = User.GetEmail();
        User user = await _userService.GetUserByEmailAsync(email);

        return Ok(user.ToDto());
    }

    [HttpPatch("subscription", Name = "toggleSubscription")]
    public async Task<ActionResult> ToggleSubscription()
    {
        string email = User.GetEmail();
        await _userService.ToggleIsSubscribedAsync(email);

        return Ok();
    }
}
