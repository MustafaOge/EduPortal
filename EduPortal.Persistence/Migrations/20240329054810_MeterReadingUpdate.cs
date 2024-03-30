using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MeterReadingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeakIndex",
                table: "MeterReadings",
                newName: "TotalLastIndex");

            migrationBuilder.RenameColumn(
                name: "NightIndex",
                table: "MeterReadings",
                newName: "TotalFirstIndex");

            migrationBuilder.RenameColumn(
                name: "LastIndex",
                table: "MeterReadings",
                newName: "PeakLastIndex");

            migrationBuilder.RenameColumn(
                name: "FirstIndex",
                table: "MeterReadings",
                newName: "PeakFirstIndex");

            migrationBuilder.RenameColumn(
                name: "DayIndex",
                table: "MeterReadings",
                newName: "NightLastIndex");

            migrationBuilder.AddColumn<decimal>(
                name: "DayFirstIndex",
                table: "MeterReadings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DayLastIndex",
                table: "MeterReadings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NightFirstIndex",
                table: "MeterReadings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayFirstIndex",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "DayLastIndex",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "NightFirstIndex",
                table: "MeterReadings");

            migrationBuilder.RenameColumn(
                name: "TotalLastIndex",
                table: "MeterReadings",
                newName: "PeakIndex");

            migrationBuilder.RenameColumn(
                name: "TotalFirstIndex",
                table: "MeterReadings",
                newName: "NightIndex");

            migrationBuilder.RenameColumn(
                name: "PeakLastIndex",
                table: "MeterReadings",
                newName: "LastIndex");

            migrationBuilder.RenameColumn(
                name: "PeakFirstIndex",
                table: "MeterReadings",
                newName: "FirstIndex");

            migrationBuilder.RenameColumn(
                name: "NightLastIndex",
                table: "MeterReadings",
                newName: "DayIndex");
        }
    }
}
