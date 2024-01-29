using app;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class migrationLab7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaneId",
                table: "Planes",
                newName: "planeId");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "Cars",
                newName: "carId");

            migrationBuilder.AddColumn<float>(
                name: "time0_100",
                table: "Cars",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time0_100",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "planeId",
                table: "Planes",
                newName: "PlaneId");

            migrationBuilder.RenameColumn(
                name: "carId",
                table: "Cars",
                newName: "CarId");
        }
    }
}
