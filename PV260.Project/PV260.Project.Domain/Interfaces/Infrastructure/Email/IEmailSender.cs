using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.Infrastructure.Email;

public interface IEmailSender
{
    Task SendAsync(EmailConfiguration configuration);
}