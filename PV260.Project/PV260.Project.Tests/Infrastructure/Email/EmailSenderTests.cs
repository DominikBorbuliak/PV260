using Microsoft.Extensions.Options;
using Moq;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Options.SMTP;
using PV260.Project.Infrastructure.Email;
using System.Reflection;

namespace PV260.Project.Tests.Infrastructure.Email;
public class EmailSenderTests
{
    private readonly SMTPOptions _validOptions = new()
    {
        Host = "smtp.example.com",
        Port = 587,
        Email = "sender@example.com",
        Password = "password"
    };

    private EmailSender CreateSender(SMTPOptions? options = null)
    {
        var optionsMock = new Mock<IOptions<SMTPOptions>>();
        optionsMock.Setup(o => o.Value).Returns(options ?? _validOptions);
        return new EmailSender(optionsMock.Object);
    }

    [Fact]
    public void EnsureSmtpConfigured_ThrowsIfHostMissing()
    {
        var options = new SMTPOptions { Host = null, Port = 587, Email = "email", Password = "pw" };
        var sender = CreateSender(options);

        var ex = Assert.Throws<TargetInvocationException>(() =>
            sender.GetType().GetMethod("EnsureSmtpConfigured", BindingFlags.NonPublic | BindingFlags.Instance)!
                  .Invoke(sender, null));

        Assert.IsType<ConfigurationException>(ex.InnerException);
        Assert.Equal("SMTP host not configured.", ex.InnerException!.Message);
    }

    [Fact]
    public void EnsureSmtpConfigured_ThrowsIfPortMissing()
    {
        var options = new SMTPOptions { Host = "smtp", Port = 0, Email = "email", Password = "pw" };
        var sender = CreateSender(options);

        var ex = Assert.Throws<TargetInvocationException>(() =>
            sender.GetType().GetMethod("EnsureSmtpConfigured", BindingFlags.NonPublic | BindingFlags.Instance)!
                  .Invoke(sender, null));

        Assert.IsType<ConfigurationException>(ex.InnerException);
        Assert.Equal("SMTP port not configured.", ex.InnerException!.Message);
    }

    [Fact]
    public void EnsureSmtpConfigured_ThrowsIfEmailMissing()
    {
        var options = new SMTPOptions { Host = "smtp", Port = 587, Email = null!, Password = "pw" };
        var sender = CreateSender(options);

        var ex = Assert.Throws<TargetInvocationException>(() =>
            sender.GetType().GetMethod("EnsureSmtpConfigured", BindingFlags.NonPublic | BindingFlags.Instance)!
                  .Invoke(sender, null));

        Assert.IsType<ConfigurationException>(ex.InnerException);
        Assert.Equal("SMTP username not configured.", ex.InnerException!.Message);
    }

    [Fact]
    public void EnsureSmtpConfigured_ThrowsIfPasswordMissing()
    {
        var options = new SMTPOptions { Host = "smtp", Port = 587, Email = "email", Password = null! };
        var sender = CreateSender(options);

        var ex = Assert.Throws<TargetInvocationException>(() =>
            sender.GetType().GetMethod("EnsureSmtpConfigured", BindingFlags.NonPublic | BindingFlags.Instance)!
                  .Invoke(sender, null));

        Assert.IsType<ConfigurationException>(ex.InnerException);
        Assert.Equal("SMTP password not configured.", ex.InnerException!.Message);
    }
}
