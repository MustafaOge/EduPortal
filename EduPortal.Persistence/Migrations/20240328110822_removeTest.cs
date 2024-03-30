using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterReadingas_Invoices_InvoiceId",
                table: "MeterReadingas");

            migrationBuilder.DropTable(
                name: "Testers");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeterReadingas",
                table: "MeterReadingas");

            migrationBuilder.RenameTable(
                name: "MeterReadingas",
                newName: "MeterReadings");

            migrationBuilder.RenameIndex(
                name: "IX_MeterReadingas_InvoiceId",
                table: "MeterReadings",
                newName: "IX_MeterReadings_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeterReadings",
                table: "MeterReadings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterReadings_Invoices_InvoiceId",
                table: "MeterReadings",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterReadings_Invoices_InvoiceId",
                table: "MeterReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeterReadings",
                table: "MeterReadings");

            migrationBuilder.RenameTable(
                name: "MeterReadings",
                newName: "MeterReadingas");

            migrationBuilder.RenameIndex(
                name: "IX_MeterReadings_InvoiceId",
                table: "MeterReadingas",
                newName: "IX_MeterReadingas_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeterReadingas",
                table: "MeterReadingas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Testers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MeterReadingas_Invoices_InvoiceId",
                table: "MeterReadingas",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
