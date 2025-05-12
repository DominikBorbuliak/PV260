using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);

    Task ToggleIsSubscribedAsync(string email);

    Task<IList<string>> GetSubscribedUserEmailsAsync();
}
