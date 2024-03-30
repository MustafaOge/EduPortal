using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addMeterReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterReadingas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalIndex = table.Column<int>(type: "int", nullable: false),
                    DayIndex = table.Column<int>(type: "int", nullable: false),
                    PeakIndex = table.Column<int>(type: "int", nullable: false),
                    NightIndex = table.Column<int>(type: "int", nullable: false),
                    FirstIndex = table.Column<int>(type: "int", nullable: false),
                    LastIndex = table.Column<int>(type: "int", nullable: false),
                    Difference = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUser = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadingas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterReadingas");
        }
    }
}
