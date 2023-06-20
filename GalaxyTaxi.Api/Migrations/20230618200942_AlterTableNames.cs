using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class AlterTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_VendorCompany_CurrentWinnerId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_CustomerCompany_CustomerCompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_CustomerCompany_CustomerCompanyId",
                table: "Journeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_VendorCompany_VendorCompanyId",
                table: "Journeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_CustomerCompany_CustomerCompanyId",
                table: "Offices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorCompany",
                table: "VendorCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerCompany",
                table: "CustomerCompany");

            migrationBuilder.RenameTable(
                name: "VendorCompany",
                newName: "VendorCompanies");

            migrationBuilder.RenameTable(
                name: "CustomerCompany",
                newName: "CustomerCompanies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorCompanies",
                table: "VendorCompanies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerCompanies",
                table: "CustomerCompanies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_VendorCompanies_CurrentWinnerId",
                table: "Auctions",
                column: "CurrentWinnerId",
                principalTable: "VendorCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_CustomerCompanies_CustomerCompanyId",
                table: "Employees",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_CustomerCompanies_CustomerCompanyId",
                table: "Journeys",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_VendorCompanies_VendorCompanyId",
                table: "Journeys",
                column: "VendorCompanyId",
                principalTable: "VendorCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_CustomerCompanies_CustomerCompanyId",
                table: "Offices",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_VendorCompanies_CurrentWinnerId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_CustomerCompanies_CustomerCompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_CustomerCompanies_CustomerCompanyId",
                table: "Journeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_VendorCompanies_VendorCompanyId",
                table: "Journeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_CustomerCompanies_CustomerCompanyId",
                table: "Offices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorCompanies",
                table: "VendorCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerCompanies",
                table: "CustomerCompanies");

            migrationBuilder.RenameTable(
                name: "VendorCompanies",
                newName: "VendorCompany");

            migrationBuilder.RenameTable(
                name: "CustomerCompanies",
                newName: "CustomerCompany");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorCompany",
                table: "VendorCompany",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerCompany",
                table: "CustomerCompany",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_VendorCompany_CurrentWinnerId",
                table: "Auctions",
                column: "CurrentWinnerId",
                principalTable: "VendorCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_CustomerCompany_CustomerCompanyId",
                table: "Employees",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_CustomerCompany_CustomerCompanyId",
                table: "Journeys",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_VendorCompany_VendorCompanyId",
                table: "Journeys",
                column: "VendorCompanyId",
                principalTable: "VendorCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_CustomerCompany_CustomerCompanyId",
                table: "Offices",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
