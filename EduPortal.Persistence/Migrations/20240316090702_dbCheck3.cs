using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dbCheck3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubsCorporates");

            migrationBuilder.DropTable(
                name: "SubsIndividuals");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_SubsCorporates_Subscriber_Id",
                        column: x => x.Id,
                        principalTable: "Subscriber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubsIndividuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsIndividuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubsIndividuals_Subscriber_Id",
                        column: x => x.Id,
                        principalTable: "Subscriber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
