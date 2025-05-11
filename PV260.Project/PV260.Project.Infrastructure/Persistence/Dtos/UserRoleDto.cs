using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Dtos;

public class UserRoleDto
{
    public UserEntity User { get; set; } = null!;
    public string RoleName { get; set; } = string.Empty;
}