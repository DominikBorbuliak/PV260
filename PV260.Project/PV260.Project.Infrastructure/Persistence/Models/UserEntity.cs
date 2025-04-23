using Microsoft.AspNetCore.Identity;

namespace PV260.Project.Infrastructure.Persistence.Models;

public class UserEntity : IdentityUser
{
    public bool IsSubscribed { get; set; }
}
