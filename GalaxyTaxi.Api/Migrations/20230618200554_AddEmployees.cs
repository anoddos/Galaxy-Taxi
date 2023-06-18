using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class AddEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_CustomerCompany_CustomerCompanyId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Office_OfficeId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAddress_Addresses_AddressId",
                table: "EmployeeAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAddress_Employee_EmployeeId",
                table: "EmployeeAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_Office_Addresses_AddressId",
                table: "Office");

            migrationBuilder.DropForeignKey(
                name: "FK_Office_CustomerCompany_CustomerCompanyId",
                table: "Office");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Office",
                table: "Office");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAddress",
                table: "EmployeeAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Office",
                newName: "Offices");

            migrationBuilder.RenameTable(
                name: "EmployeeAddress",
                newName: "EmployeeAddresses");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Office_CustomerCompanyId",
                table: "Offices",
                newName: "IX_Offices_CustomerCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Office_AddressId",
                table: "Offices",
                newName: "IX_Offices_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeAddress_EmployeeId",
                table: "EmployeeAddresses",
                newName: "IX_EmployeeAddresses_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeAddress_AddressId",
                table: "EmployeeAddresses",
                newName: "IX_EmployeeAddresses_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_OfficeId",
                table: "Employees",
                newName: "IX_Employees_OfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_CustomerCompanyId",
                table: "Employees",
                newName: "IX_Employees_CustomerCompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offices",
                table: "Offices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAddresses",
                table: "EmployeeAddresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAddresses_Addresses_AddressId",
                table: "EmployeeAddresses",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAddresses_Employees_EmployeeId",
                table: "EmployeeAddresses",
                column: "EmployeeId",
                principalTable: "Employees",
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
                name: "FK_Employees_Offices_OfficeId",
                table: "Employees",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Addresses_AddressId",
                table: "Offices",
                column: "AddressId",
                principalTable: "Addresses",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAddresses_Addresses_AddressId",
                table: "EmployeeAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAddresses_Employees_EmployeeId",
                table: "EmployeeAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_CustomerCompany_CustomerCompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Offices_OfficeId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Addresses_AddressId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_CustomerCompany_CustomerCompanyId",
                table: "Offices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offices",
                table: "Offices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAddresses",
                table: "EmployeeAddresses");

            migrationBuilder.RenameTable(
                name: "Offices",
                newName: "Office");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "EmployeeAddresses",
                newName: "EmployeeAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Offices_CustomerCompanyId",
                table: "Office",
                newName: "IX_Office_CustomerCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Offices_AddressId",
                table: "Office",
                newName: "IX_Office_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_OfficeId",
                table: "Employee",
                newName: "IX_Employee_OfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_CustomerCompanyId",
                table: "Employee",
                newName: "IX_Employee_CustomerCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddress",
                newName: "IX_EmployeeAddress_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeAddresses_AddressId",
                table: "EmployeeAddress",
                newName: "IX_EmployeeAddress_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Office",
                table: "Office",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAddress",
                table: "EmployeeAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_CustomerCompany_CustomerCompanyId",
                table: "Employee",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Office_OfficeId",
                table: "Employee",
                column: "OfficeId",
                principalTable: "Office",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAddress_Addresses_AddressId",
                table: "EmployeeAddress",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAddress_Employee_EmployeeId",
                table: "EmployeeAddress",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Office_Addresses_AddressId",
                table: "Office",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Office_CustomerCompany_CustomerCompanyId",
                table: "Office",
                column: "CustomerCompanyId",
                principalTable: "CustomerCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
