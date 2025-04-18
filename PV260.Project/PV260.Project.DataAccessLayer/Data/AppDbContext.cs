﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PV260.Project.DataAccessLayer.Models;

namespace PV260.Project.DataAccessLayer.Data;

public class AppDbContext : IdentityDbContext<User, Role, string>
{
    public override DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}