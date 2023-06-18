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
                name: "FK_CustomerCompany_Accounts_AccountId1",
                table: "CustomerCompany");

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

            migrationBuilder.DropForeignKey(
                name: "FK_VendorCompany_Accounts_AccountId1",
                table: "VendorCompany");

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

            migrationBuilder.RenameIndex(
                name: "IX_VendorCompany_AccountId1",
                table: "VendorCompanies",
                newName: "IX_VendorCompanies_AccountId1");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCompany_AccountId1",
                table: "CustomerCompanies",
                newName: "IX_CustomerCompanies_AccountId1");

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
                name: "FK_CustomerCompanies_Accounts_AccountId1",
                table: "CustomerCompanies",
                column: "AccountId1",
                principalTable: "Accounts",
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

            migrationBuilder.AddForeignKey(
                name: "FK_VendorCompanies_Accounts_AccountId1",
                table: "VendorCompanies",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_VendorCompanies_CurrentWinnerId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCompanies_Accounts_AccountId1",
                table: "CustomerCompanies");

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

            migrationBuilder.DropForeignKey(
                name: "FK_VendorCompanies_Accounts_AccountId1",
                table: "VendorCompanies");

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

            migrationBuilder.RenameIndex(
                name: "IX_VendorCompanies_AccountId1",
                table: "VendorCompany",
                newName: "IX_VendorCompany_AccountId1");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCompanies_AccountId1",
                table: "CustomerCompany",
                newName: "IX_CustomerCompany_AccountId1");

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
                name: "FK_CustomerCompany_Accounts_AccountId1",
                table: "CustomerCompany",
                column: "AccountId1",
                principalTable: "Accounts",
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

            migrationBuilder.AddForeignKey(
                name: "FK_VendorCompany_Accounts_AccountId1",
                table: "VendorCompany",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
