using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Components.Common.Controllers;
using PV260.Project.Components.UsersComponent.Dtos;
using PV260.Project.Components.UsersComponent.Extensions;
using PV260.Project.Components.UsersComponent.Mappers;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent.Controllers;

[Authorize]
public class UserController : ApiController
{
    private readonly IUserComponent _userComponent;

    public UserController(IUserComponent userComponent)
    {
        _userComponent = userComponent;
    }

    [HttpPost("logout", Name = "logout")]
    public async Task<ActionResult> Logout()
    {
        await _userComponent.SignOutAsync();

        return Ok();
    }

    [HttpGet("me", Name = "getMe")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        string email = User.GetEmail();
        User user = await _userComponent.GetUserByEmailAsync(email);

        return Ok(user.ToDto());
    }

    [HttpPatch("subscription", Name = "toggleSubscription")]
    public async Task<ActionResult> ToggleSubscription()
    {
        string email = User.GetEmail();
        await _userComponent.ToggleIsSubscribedAsync(email);

        return Ok();
    }
}
