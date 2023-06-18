using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class AddJournies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Journeys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journeys_CustomerCompany_CustomerCompanyId",
                        column: x => x.CustomerCompanyId,
                        principalTable: "CustomerCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Journeys_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Journeys_VendorCompany_VendorCompanyId",
                        column: x => x.VendorCompanyId,
                        principalTable: "VendorCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JourneyId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeAddressId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stops_EmployeeAddresses_EmployeeAddressId",
                        column: x => x.EmployeeAddressId,
                        principalTable: "EmployeeAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stops_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_CustomerCompanyId",
                table: "Journeys",
                column: "CustomerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_OfficeId",
                table: "Journeys",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_VendorCompanyId",
                table: "Journeys",
                column: "VendorCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_EmployeeAddressId",
                table: "Stops",
                column: "EmployeeAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_JourneyId",
                table: "Stops",
                column: "JourneyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Journeys");
        }
    }
}
