using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changecolumntypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleInsurances_ExpiryDateUtc",
                table: "VehicleInsurances");

            migrationBuilder.DropColumn(
                name: "ExpiryDateUtc",
                table: "VehicleTechnicalInspections");

            migrationBuilder.DropColumn(
                name: "ExpiryDateUtc",
                table: "VehicleInsurances");

            migrationBuilder.AlterColumn<long>(
                name: "VehicleId",
                table: "VehicleTechnicalInspections",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpiryDate",
                table: "VehicleTechnicalInspections",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<long>(
                name: "VehicleId",
                table: "VehicleInsurances",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "VehicleInsurances",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInsurances_VehicleId",
                table: "VehicleInsurances",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleInsurances_Vehicles_VehicleId",
                table: "VehicleInsurances",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleInsurances_Vehicles_VehicleId",
                table: "VehicleInsurances");

            migrationBuilder.DropIndex(
                name: "IX_VehicleInsurances_VehicleId",
                table: "VehicleInsurances");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "VehicleTechnicalInspections");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "VehicleInsurances");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "VehicleTechnicalInspections",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDateUtc",
                table: "VehicleTechnicalInspections",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "VehicleInsurances",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDateUtc",
                table: "VehicleInsurances",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInsurances_ExpiryDateUtc",
                table: "VehicleInsurances",
                column: "ExpiryDateUtc");
        }
    }
}
