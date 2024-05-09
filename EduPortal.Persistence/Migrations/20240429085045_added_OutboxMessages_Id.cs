using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_OutboxMessages_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "IdempotentToken",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OutboxMessages",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "OutboxMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUser",
                table: "OutboxMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "OutboxMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedByUser",
                table: "OutboxMessages",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "CreatedByUser",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "UpdatedByUser",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "IdempotentToken",
                table: "OutboxMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages",
                column: "IdempotentToken");
        }
    }
}
