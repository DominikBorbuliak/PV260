using PV260.Project.Domain.Interfaces.Persistence;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
    }

    public async Task ToggleIsSubscribedAsync(string email)
    {
        await _unitOfWork.UserRepository.ToggleIsSubscribedAsync(email);
    }

    public async Task<IList<string>> GetSubscribedEmailsAsync()
    {
        return await _unitOfWork.UserRepository.GetSubscribedUserEmailsAsync();
    }
}
