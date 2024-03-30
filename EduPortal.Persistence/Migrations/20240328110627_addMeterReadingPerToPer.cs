using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addMeterReadingPerToPer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MeterReadingas_InvoiceId",
                table: "MeterReadingas",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeterReadingas_Invoices_InvoiceId",
                table: "MeterReadingas",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterReadingas_Invoices_InvoiceId",
                table: "MeterReadingas");

            migrationBuilder.DropIndex(
                name: "IX_MeterReadingas_InvoiceId",
                table: "MeterReadingas");
        }
    }
}
