using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PV260.Project.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddDiffReportsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Reports",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ReportJson = table.Column<string>(type: "TEXT", nullable: false),
                DiffJson = table.Column<string>(type: "TEXT", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Reports", x => x.Id));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Reports");
    }
}
