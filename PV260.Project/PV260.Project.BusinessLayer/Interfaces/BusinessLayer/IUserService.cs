using PV260.Project.BusinessLayer.Models;

namespace PV260.Project.BusinessLayer.Interfaces.BusinessLayer;

public interface IUserService
{
    Task<User> GetUserByEmailAsync(string email);

    Task ToggleIsSubscribedAsync(string email);
}
