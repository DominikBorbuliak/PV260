using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PV260.Project.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddIsSubscribedToUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<bool>(
            name: "IsSubscribed",
            table: "AspNetUsers",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "IsSubscribed",
            table: "AspNetUsers");
    }
}
