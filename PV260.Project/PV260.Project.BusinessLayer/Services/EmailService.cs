using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PV260.Project.BusinessLayer.Exceptions;
using PV260.Project.BusinessLayer.Interfaces.BusinessLayer.Services;
using PV260.Project.BusinessLayer.Models;
using PV260.Project.BusinessLayer.Options.SMTP;

namespace PV260.Project.BusinessLayer.Services;

public class EmailService : IEmailService
{
    private readonly SMTPOptions _smtpOptions;
    
    public EmailService(IOptions<SMTPOptions> smtpCredentialsOptions)
    {
        _smtpOptions = smtpCredentialsOptions.Value;
    }
    
    /// <summary>
    /// Sends an email using the configured SMTP server.
    /// </summary>
    /// <remarks>
    /// This method requires 'SMTP' options to be configured. (Host, Port, Email, Password)
    /// </remarks>
    /// <param name="configuration">Email configuration.</param>
    public async Task SendAsync(EmailConfiguration configuration)
    {
        EnsureSmtpConfigured();
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_smtpOptions.Email));

        foreach (var recipient in configuration.Recipients)    
        {
            email.To.Add(MailboxAddress.Parse(recipient));
        }

        email.Subject = configuration.Subject;
        email.Body = new TextPart(configuration.Format) { Text = configuration.Message };

        using var smtpClient = new SmtpClient();
        
        await smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync(_smtpOptions.Email, _smtpOptions.Password);
        await smtpClient.SendAsync(email);
        await smtpClient.DisconnectAsync(true);
    }

    private void EnsureSmtpConfigured()
    {
        if (_smtpOptions.Host is null)
        {
            throw new ConfigurationException("SMTP host not configured.");
        }
        
        if (_smtpOptions.Port == default)
        {
            throw new ConfigurationException("SMTP port not configured.");
        }

        if (_smtpOptions.Password is null)
        {
            throw new ConfigurationException("SMTP password not configured.");
        }
        
        if (_smtpOptions.Email is null)
        {
            throw new ConfigurationException("SMTP username not configured.");
        }
    }
}