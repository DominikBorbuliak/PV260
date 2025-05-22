using PV260.Project.Domain.Exceptions;
using System.Security.Claims;
using PV260.Project.Domain;
using PV260.Project.Components.UsersComponent.Extensions;

namespace PV260.Project.Tests.Components.UsersComponent;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void EmailClaimExists_ShouldReturnEmail()
    {
        _ = new When()
            .WithEmail("user@example.com")
            .ThenCallGetEmail()
            .ShouldReturn("user@example.com");
    }

    [Fact]
    public void MissingEmailClaim_ShouldThrowUnauthorized()
    {
        _ = new When()
            .ThenCallGetEmail()
            .ShouldThrowUnauthorizedException();
    }

    [Fact]
    public void MultipleEmailClaims_ShouldReturnFirst()
    {
        _ = new When()
            .WithEmail("first@example.com")
            .WithEmail("second@example.com")
            .ThenCallGetEmail()
            .ShouldReturn("first@example.com");
    }

    private sealed class When
    {
        private readonly ClaimsPrincipalBuilder _builder = new();
        private string? _result;
        private Exception? _exception;

        public When WithEmail(string email)
        {
            _builder.WithEmail(email);
            return this;
        }

        public When ThenCallGetEmail()
        {
            var principal = _builder.Build();

            try
            {
                _result = principal.GetEmail();
            }
            catch (Exception ex)
            {
                _exception = ex;
            }

            return this;
        }

        public When ShouldReturn(string expected)
        {
            Assert.Null(_exception);
            Assert.Equal(expected, _result);
            return this;
        }

        public When ShouldThrowUnauthorizedException()
        {
            Assert.NotNull(_exception);
            Assert.IsType<UnauthorizedException>(_exception);
            Assert.Equal(Constants.Error.Unauthorized, _exception.Message);
            return this;
        }
    }

    private sealed class ClaimsPrincipalBuilder
    {
        private readonly List<Claim> _claims = new();

        public ClaimsPrincipalBuilder WithEmail(string email)
        {
            _claims.Add(new Claim(ClaimTypes.Email, email));
            return this;
        }

        public ClaimsPrincipal Build()
        {
            var identity = new ClaimsIdentity(_claims);
            return new ClaimsPrincipal(identity);
        }
    }
}
