using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryFieldsFromVehiclePart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastServiceDate",
                table: "VehicleParts");

            migrationBuilder.DropColumn(
                name: "NextServiceDate",
                table: "VehicleParts");

            migrationBuilder.DropColumn(
                name: "ServiceIntervalKm",
                table: "VehicleParts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastServiceDate",
                table: "VehicleParts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextServiceDate",
                table: "VehicleParts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceIntervalKm",
                table: "VehicleParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
