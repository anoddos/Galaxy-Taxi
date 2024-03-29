﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    public partial class MakeFulfilmentPercentageNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FulfillmentPercentage",
                table: "Auctions",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FulfillmentPercentage",
                table: "Auctions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
