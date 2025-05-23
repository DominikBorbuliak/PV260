﻿namespace PV260.Project.Domain.Models;

public class User
{
    public required string Email { get; set; }

    public bool IsSubscribed { get; set; }

    public string? Role { get; set; }
}
