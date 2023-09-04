using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class AddAditionalFeatureSupports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOfficeDest",
                table: "Journeys",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportTwoWayJourneys",
                table: "CustomerCompanies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FulfillmentPercentage",
                table: "Auctions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOfficeDest",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "SupportTwoWayJourneys",
                table: "CustomerCompanies");

            migrationBuilder.DropColumn(
                name: "FulfillmentPercentage",
                table: "Auctions");
        }
    }
}
