using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dbCheck4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Subscriber_SubscriberId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriber",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "CorprorateName",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "NameSurname",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "TaxIdNumber",
                table: "Subscriber");

            migrationBuilder.RenameTable(
                name: "Subscriber",
                newName: "Subscribers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SubsCorporates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CorprorateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsCorporates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubsCorporates_Subscribers_Id",
                        column: x => x.Id,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubsIndividuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsIndividuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubsIndividuals_Subscribers_Id",
                        column: x => x.Id,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Subscribers_SubscriberId",
                table: "Invoices",
                column: "SubscriberId",
                principalTable: "Subscribers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Subscribers_SubscriberId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "SubsCorporates");

            migrationBuilder.DropTable(
                name: "SubsIndividuals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.RenameTable(
                name: "Subscribers",
                newName: "Subscriber");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Subscriber",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorprorateName",
                table: "Subscriber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Subscriber",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Subscriber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSurname",
                table: "Subscriber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxIdNumber",
                table: "Subscriber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriber",
                table: "Subscriber",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Subscriber_SubscriberId",
                table: "Invoices",
                column: "SubscriberId",
                principalTable: "Subscriber",
                principalColumn: "Id");
        }
    }
}
