using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent.Services;

public interface IUserService
{
    Task<User> GetUserByEmailAsync(string email);

    Task ToggleIsSubscribedAsync(string email);

    Task<IList<string>> GetSubscribedEmailsAsync();
}
