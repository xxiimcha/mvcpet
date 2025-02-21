using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPET.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectionReasonToAdoptionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "AdoptionRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "AdoptionRequests");
        }
    }
}
