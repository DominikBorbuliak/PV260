using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.Domain;

public interface IUserService
{
    Task<User> GetUserByEmailAsync(string email);

    Task ToggleIsSubscribedAsync(string email);
}
