using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngleSharp1.Migrations
{
    /// <inheritdoc />
    public partial class PriceToProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Smartphones",
                newName: "Processor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Processor",
                table: "Smartphones",
                newName: "Price");
        }
    }
}
