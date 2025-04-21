using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.BusinessLayer.Interfaces.BusinessLayer;
using PV260.Project.BusinessLayer.Models;
using PV260.Project.DataAccessLayer.Models;
using PV260.Project.Server.Dtos;
using PV260.Project.Server.Extensions;
using PV260.Project.Server.Mappers;

namespace PV260.Project.Server.Controllers;

public class UserController : ApiController
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IUserService _userService;
    public UserController(SignInManager<UserEntity> signInManager, IUserService userService)
    {
        _signInManager = signInManager;
        _userService = userService;
    }

    [Authorize]
    [HttpPost("logout", Name = "logout")]
    public async Task<IResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Results.Ok();
    }

    [Authorize]
    [HttpGet("pingauth", Name = "pingauth")]
    public async Task<ActionResult<UserDto>> PingAuth()
    {
        string email = User.GetEmail();
        User user = await _userService.GetUserByEmailAsync(email);

        return Ok(user.ToDto());
    }

    [Authorize]
    [HttpPatch("toggleIsSubscribed", Name = "toggleIsSubscribed")]
    public async Task<ActionResult> ToggleIsSubscired()
    {
        string email = User.GetEmail();
        await _userService.ToggleIsSubscribedAsync(email);

        return Ok();
    }
}
