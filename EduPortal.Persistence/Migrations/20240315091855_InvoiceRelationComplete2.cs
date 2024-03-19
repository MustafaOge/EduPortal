using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceRelationComplete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Subscriber_SubscriberId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "Invoice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Subscriber_SubscriberId",
                table: "Invoice",
                column: "SubscriberId",
                principalTable: "Subscriber",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Subscriber_SubscriberId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Subscriber_SubscriberId",
                table: "Invoice",
                column: "SubscriberId",
                principalTable: "Subscriber",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
