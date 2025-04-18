using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.DataAccessLayer.Models;
using System.Security.Claims;

namespace PV260.Project.Server.Controllers;
public class UserController : ApiController
{
    private readonly SignInManager<User> _signInManager;

    public UserController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
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
    public IResult PingAuth()
    {
        string? email = User.FindFirstValue(ClaimTypes.Email);
        return Results.Json(new { Email = email });
    }
}
