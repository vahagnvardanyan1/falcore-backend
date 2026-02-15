using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleInsurances_Vehicles_CehicleId",
                table: "VehicleInsurances");

            migrationBuilder.DropIndex(
                name: "IX_VehicleInsurances_CehicleId",
                table: "VehicleInsurances");

            migrationBuilder.DropColumn(
                name: "CehicleId",
                table: "VehicleInsurances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CehicleId",
                table: "VehicleInsurances",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInsurances_CehicleId",
                table: "VehicleInsurances",
                column: "CehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleInsurances_Vehicles_CehicleId",
                table: "VehicleInsurances",
                column: "CehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }
    }
}
