using PV260.Project.BusinessLayer.Models;

namespace PV260.Project.BusinessLayer.Interfaces.BusinessLayer.Services;

public interface IEmailService
{
    Task SendAsync(EmailConfiguration configuration);
}