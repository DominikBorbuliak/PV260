namespace PV260.Project.Components.UsersComponent.Dtos;

public class UserDto
{
    public required string Email { get; set; }

    public bool IsSubscribed { get; set; }

    public string? Role { get; set; }
}