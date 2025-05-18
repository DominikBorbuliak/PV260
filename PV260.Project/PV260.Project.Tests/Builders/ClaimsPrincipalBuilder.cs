using System.Security.Claims;

namespace PV260.Project.Tests.Builders;

public class ClaimsPrincipalBuilder
{
    private readonly List<Claim> _claims = new();

    public ClaimsPrincipalBuilder WithEmail(string email)
    {
        _claims.Add(new Claim(ClaimTypes.Email, email));
        return this;
    }

    public ClaimsPrincipalBuilder WithClaim(string type, string value)
    {
        _claims.Add(new Claim(type, value));
        return this;
    }

    public ClaimsPrincipal Build()
    {
        var identity = new ClaimsIdentity(_claims);
        return new ClaimsPrincipal(identity);
    }
}

