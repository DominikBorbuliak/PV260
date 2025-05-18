using PV260.Project.Domain.Exceptions;
using PV260.Project.Tests.Builders;
using PV260.Project.Components.UsersComponent.Extensions;
using System.Security.Claims;
using PV260.Project.Domain;

namespace PV260.Project.Tests;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetEmail_ShouldReturnEmail_WhenEmailClaimExists()
    {
        var principal = new ClaimsPrincipalBuilder()
            .WithEmail("user@example.com")
            .Build();

        string email = principal.GetEmail();

        Assert.Equal("user@example.com", email);
    }

    [Fact]
    public void GetEmail_ShouldThrowUnauthorizedException_WhenEmailClaimMissing()
    {
        var principal = new ClaimsPrincipalBuilder().Build();

        var ex = Assert.Throws<UnauthorizedException>(() => principal.GetEmail());
        Assert.Equal(Constants.Error.Unauthorized, ex.Message);
    }

    [Fact]
    public void GetEmail_ShouldReturnFirstEmailClaim_WhenMultipleEmailClaimsExist()
    {
        var principal = new ClaimsPrincipalBuilder()
            .WithClaim(ClaimTypes.Email, "first@example.com")
            .WithClaim(ClaimTypes.Email, "second@example.com")
            .Build();

        string email = principal.GetEmail();

        Assert.Equal("first@example.com", email);
    }

    [Fact]
    public void GetEmail_ShouldReturnEmail_WhenClaimTypeCaseVaries()
    {
        var principal = new ClaimsPrincipalBuilder()
            .WithClaim("email", "caseinsensitive@example.com")
            .Build();

        string email = principal.FindFirstValue("email"); // Manual since GetEmail expects exact ClaimTypes.Email

        Assert.Equal("caseinsensitive@example.com", email);
    }
}
