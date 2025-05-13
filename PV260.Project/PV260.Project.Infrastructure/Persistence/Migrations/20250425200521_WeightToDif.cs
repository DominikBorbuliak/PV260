using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PV260.Project.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class WeightToDif : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<decimal>(
            name: "NewWeight",
            table: "ReportChangeEntity",
            type: "TEXT",
            nullable: false,
            defaultValue: 0m);

        _ = migrationBuilder.AddColumn<decimal>(
            name: "OldWeight",
            table: "ReportChangeEntity",
            type: "TEXT",
            nullable: false,
            defaultValue: 0m);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "NewWeight",
            table: "ReportChangeEntity");

        _ = migrationBuilder.DropColumn(
            name: "OldWeight",
            table: "ReportChangeEntity");
    }
}
