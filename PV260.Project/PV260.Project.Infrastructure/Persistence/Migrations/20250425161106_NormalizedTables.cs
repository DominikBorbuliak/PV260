using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PV260.Project.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class NormalizedTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "DiffJson",
            table: "Reports");

        _ = migrationBuilder.DropColumn(
            name: "ReportJson",
            table: "Reports");

        _ = migrationBuilder.CreateTable(
            name: "ReportHoldingEntity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ReportId = table.Column<Guid>(type: "TEXT", nullable: false),
                Ticker = table.Column<string>(type: "TEXT", nullable: false),
                Company = table.Column<string>(type: "TEXT", nullable: false),
                Shares = table.Column<int>(type: "INTEGER", nullable: false),
                Weight = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ReportHoldingEntity", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ReportHoldingEntity_Reports_ReportId",
                    column: x => x.ReportId,
                    principalTable: "Reports",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "ReportChangeEntity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ReportId = table.Column<Guid>(type: "TEXT", nullable: false),
                Ticker = table.Column<string>(type: "TEXT", nullable: false),
                Company = table.Column<string>(type: "TEXT", nullable: false),
                ChangeType = table.Column<int>(type: "INTEGER", nullable: false),
                OldShares = table.Column<int>(type: "INTEGER", nullable: false),
                NewShares = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_ReportChangeEntity", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_ReportChangeEntity_Reports_ReportId",
                    column: x => x.ReportId,
                    principalTable: "Reports",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_ReportHoldingEntity_ReportId",
            table: "ReportHoldingEntity",
            column: "ReportId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_ReportChangeEntity_ReportId",
            table: "ReportChangeEntity",
            column: "ReportId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "ReportHoldingEntity");

        _ = migrationBuilder.DropTable(
            name: "ReportChangeEntity");

        _ = migrationBuilder.AddColumn<string>(
            name: "DiffJson",
            table: "Reports",
            type: "TEXT",
            nullable: false,
            defaultValue: "");

        _ = migrationBuilder.AddColumn<string>(
            name: "ReportJson",
            table: "Reports",
            type: "TEXT",
            nullable: false,
            defaultValue: "");
    }
}
