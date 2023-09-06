using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class paymenttoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentToken",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentToken",
                table: "Accounts");
        }
    }
}
