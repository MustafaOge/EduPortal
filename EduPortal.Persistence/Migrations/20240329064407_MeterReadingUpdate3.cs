using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MeterReadingUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Difference",
                table: "MeterReadings",
                newName: "TotalDifference");

            migrationBuilder.AddColumn<decimal>(
                name: "DayDifference",
                table: "MeterReadings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NightDifference",
                table: "MeterReadings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PeakDifference",
                table: "MeterReadings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReadingDayDifference",
                table: "MeterReadings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayDifference",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "NightDifference",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "PeakDifference",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "ReadingDayDifference",
                table: "MeterReadings");

            migrationBuilder.RenameColumn(
                name: "TotalDifference",
                table: "MeterReadings",
                newName: "Difference");
        }
    }
}
