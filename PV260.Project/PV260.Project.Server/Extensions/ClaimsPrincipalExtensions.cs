using PV260.Project.Domain;
using PV260.Project.Domain.Exceptions;
using System.Security.Claims;

namespace PV260.Project.Server.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal
            .FindFirstValue(ClaimTypes.Email)
            ?? throw new UnauthorizedException(Constants.Error.Unauthorized);
    }
}
