namespace PV260.Project.Server.Dtos;

public class UserDto
{
    public required string Email { get; set; }

    public bool IsSubscribed { get; set; }
}
