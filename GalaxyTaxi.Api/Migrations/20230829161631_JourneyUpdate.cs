using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class JourneyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_VendorCompanies_VendorCompanyId",
                table: "Journeys");

            migrationBuilder.DropIndex(
                name: "IX_Journeys_VendorCompanyId",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "VendorCompanyId",
                table: "Journeys");

            migrationBuilder.AddColumn<bool>(
                name: "HasActiveJourney",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CustomerCompanyId",
                table: "Auctions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDetected",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CustomerCompanyId",
                table: "Auctions",
                column: "CustomerCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_CustomerCompanies_CustomerCompanyId",
                table: "Auctions",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_CustomerCompanies_CustomerCompanyId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CustomerCompanyId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "HasActiveJourney",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CustomerCompanyId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "IsDetected",
                table: "Addresses");

            migrationBuilder.AddColumn<long>(
                name: "VendorCompanyId",
                table: "Journeys",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_VendorCompanyId",
                table: "Journeys",
                column: "VendorCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_VendorCompanies_VendorCompanyId",
                table: "Journeys",
                column: "VendorCompanyId",
                principalTable: "VendorCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
