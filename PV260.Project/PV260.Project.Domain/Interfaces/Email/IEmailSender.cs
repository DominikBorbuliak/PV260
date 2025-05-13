using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.Email;

public interface IEmailSender
{
    Task SendAsync(EmailConfiguration configuration);
}