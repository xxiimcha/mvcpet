using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPET.Migrations
{
    /// <inheritdoc />
    public partial class AddAdoptionRequestForeignKeyToPets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdoptionRequestId",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_AdoptionRequestId",
                table: "Pets",
                column: "AdoptionRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AdoptionRequests_AdoptionRequestId",
                table: "Pets",
                column: "AdoptionRequestId",
                principalTable: "AdoptionRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AdoptionRequests_AdoptionRequestId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_AdoptionRequestId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "AdoptionRequestId",
                table: "Pets");
        }
    }
}
