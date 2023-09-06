using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class EditAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationRequestDate",
                table: "VendorCompanies");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationRequestDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationRequestDate",
                table: "Accounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationRequestDate",
                table: "VendorCompanies",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
