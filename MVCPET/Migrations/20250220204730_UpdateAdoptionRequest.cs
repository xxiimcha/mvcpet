using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPET.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdoptionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AdoptionRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AgreesToInterview",
                table: "AdoptionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AdoptionRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AdoptionRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasAdoptedBefore",
                table: "AdoptionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HouseholdMembers",
                table: "AdoptionRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdoptingForAnother",
                table: "AdoptionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRenting",
                table: "AdoptionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LivingType",
                table: "AdoptionRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "AdoptionRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AdoptionRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "AdoptionRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PetPreference",
                table: "AdoptionRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AdoptionRequests",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pronouns",
                table: "AdoptionRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "AgreesToInterview",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "HasAdoptedBefore",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "HouseholdMembers",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "IsAdoptingForAnother",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "IsRenting",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "LivingType",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "PetPreference",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Pronouns",
                table: "AdoptionRequests");
        }
    }
}
