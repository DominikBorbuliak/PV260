using PV260.Project.BusinessLayer.Models;

namespace PV260.Project.BusinessLayer.Interfaces.DataAccessLayer;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);

    Task ToggleIsSubscribedAsync(string email);
}
