using Microsoft.Extensions.Options;
using Moq;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Options.SMTP;
using PV260.Project.Infrastructure.Email;
using System.Reflection;

namespace PV260.Project.Tests.Infrastructure.Email;

public class EmailSenderTests
{
    [Fact]
    public void HostMissing_ShouldThrowConfigurationException()
    {
        _ = new When()
            .WithSMTPOptions(host: null)
            .CreateSender()
            .InvokeEnsureSmtpConfigured()
            .ThenThrows<ConfigurationException>("SMTP host not configured.");
    }

    [Fact]
    public void PortMissing_ShouldThrowConfigurationException()
    {
        _ = new When()
            .WithSMTPOptions(port: 0)
            .CreateSender()
            .InvokeEnsureSmtpConfigured()
            .ThenThrows<ConfigurationException>("SMTP port not configured.");
    }

    [Fact]
    public void EmailMissing_ShouldThrowConfigurationException()
    {
        _ = new When()
            .WithSMTPOptions(email: null)
            .CreateSender()
            .InvokeEnsureSmtpConfigured()
            .ThenThrows<ConfigurationException>("SMTP username not configured.");
    }

    [Fact]
    public void PasswordMissing_ShouldThrowConfigurationException()
    {
        _ = new When()
            .WithSMTPOptions(password: null)
            .CreateSender()
            .InvokeEnsureSmtpConfigured()
            .ThenThrows<ConfigurationException>("SMTP password not configured.");
    }

    private sealed class When
    {
        private SMTPOptions _smtpOptions = new()
        {
            Host = "smtp.example.com",
            Port = 587,
            Email = "sender@example.com",
            Password = "password"
        };

        private EmailSender _sender;
        private Exception _caughtException;

        public When WithSMTPOptions(string? host = "smtp.example.com", int port = 587, string? email = "sender@example.com", string? password = "password")
        {
            _smtpOptions = new SMTPOptions
            {
                Host = host,
                Port = port,
                Email = email,
                Password = password
            };
            return this;
        }

        public When CreateSender()
        {
            var optionsMock = new Mock<IOptions<SMTPOptions>>();
            optionsMock.Setup(o => o.Value).Returns(_smtpOptions);
            _sender = new EmailSender(optionsMock.Object);
            return this;
        }

        public When InvokeEnsureSmtpConfigured()
        {
            try
            {
                var method = typeof(EmailSender).GetMethod("EnsureSmtpConfigured", BindingFlags.NonPublic | BindingFlags.Instance);
                method!.Invoke(_sender, null);
            }
            catch (TargetInvocationException tie)
            {
                _caughtException = tie.InnerException!;
            }
            return this;
        }

        public When ThenThrows<TException>(string expectedMessage) where TException : Exception
        {
            Assert.NotNull(_caughtException);
            Assert.IsType<TException>(_caughtException);
            Assert.Equal(expectedMessage, _caughtException.Message);
            return this;
        }
    }
}
