using PV260.Project.Components.UsersComponent.Dtos;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.UsersComponent.Mappers;

internal static class UserDtoMapper
{
    public static UserDto ToDto(this User source)
    {
        return new UserDto
        {
            Email = source.Email,
            IsSubscribed = source.IsSubscribed,
            Role = source.Role,
        };
    }
}
