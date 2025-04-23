using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Mappers;

public static class UserMapper
{
    public static User ToDomainModel(this UserEntity source)
    {
        return new User
        {
            Email = source.Email ?? string.Empty,
            IsSubscribed = source.IsSubscribed
        };
    }
}
