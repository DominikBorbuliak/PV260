using PV260.Project.BusinessLayer.Models;
using PV260.Project.DataAccessLayer.Models;

namespace PV260.Project.DataAccessLayer.Mappers;

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
