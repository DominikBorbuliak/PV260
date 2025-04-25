using PV260.Project.Domain.Models;
using PV260.Project.Server.Dtos;

namespace PV260.Project.Server.Mappers;

public static class UserDtoMapper
{
    public static UserDto ToDto(this User source)
    {
        return new UserDto
        {
            Email = source.Email,
            IsSubscribed = source.IsSubscribed
        };
    }
}
