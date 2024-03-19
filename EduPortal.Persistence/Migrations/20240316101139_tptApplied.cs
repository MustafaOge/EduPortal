using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tptApplied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "CorprorateName",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "NameSurname",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "TaxIdNumber",
                table: "Subscribers");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Subscribers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubsCorporates");

            migrationBuilder.DropTable(
                name: "SubsIndividuals");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Subscribers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Subscribers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorprorateName",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSurname",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxIdNumber",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
