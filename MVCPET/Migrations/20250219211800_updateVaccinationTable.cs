using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPET.Migrations
{
    /// <inheritdoc />
    public partial class updateVaccinationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetVaccination_Pets_PetId",
                table: "PetVaccination");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetVaccination",
                table: "PetVaccination");

            migrationBuilder.DropColumn(
                name: "VaccineHistory",
                table: "PetVaccination");

            migrationBuilder.RenameTable(
                name: "PetVaccination",
                newName: "Vaccinations");

            migrationBuilder.RenameIndex(
                name: "IX_PetVaccination_PetId",
                table: "Vaccinations",
                newName: "IX_Vaccinations_PetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vaccinations",
                table: "Vaccinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinations_Pets_PetId",
                table: "Vaccinations",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinations_Pets_PetId",
                table: "Vaccinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vaccinations",
                table: "Vaccinations");

            migrationBuilder.RenameTable(
                name: "Vaccinations",
                newName: "PetVaccination");

            migrationBuilder.RenameIndex(
                name: "IX_Vaccinations_PetId",
                table: "PetVaccination",
                newName: "IX_PetVaccination_PetId");

            migrationBuilder.AddColumn<string>(
                name: "VaccineHistory",
                table: "PetVaccination",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetVaccination",
                table: "PetVaccination",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PetVaccination_Pets_PetId",
                table: "PetVaccination",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
