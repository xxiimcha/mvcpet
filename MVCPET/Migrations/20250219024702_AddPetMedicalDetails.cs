using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPET.Migrations
{
    /// <inheritdoc />
    public partial class AddPetMedicalDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Neuter",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PetVaccination",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccineHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetVaccination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetVaccination_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetVaccination_PetId",
                table: "PetVaccination",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetVaccination");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Neuter",
                table: "Pets");
        }
    }
}
