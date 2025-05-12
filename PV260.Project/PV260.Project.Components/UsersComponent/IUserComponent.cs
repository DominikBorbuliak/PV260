using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent;

public interface IUserComponent
{
    Task<User> GetUserByEmailAsync(string email);

    Task ToggleIsSubscribedAsync(string email);

    Task<IList<string>> GetSubscribedEmailsAsync();
}