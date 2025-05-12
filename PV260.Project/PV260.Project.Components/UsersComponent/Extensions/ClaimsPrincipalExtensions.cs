using System.Security.Claims;
using PV260.Project.Domain;
using PV260.Project.Domain.Exceptions;

namespace PV260.Project.Components.UsersComponent.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal
            .FindFirstValue(ClaimTypes.Email)
            ?? throw new UnauthorizedException(Constants.Error.Unauthorized);
    }
}
