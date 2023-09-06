using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class verificationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
