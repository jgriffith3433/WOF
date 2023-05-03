using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StockUnitsAndProductSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Units",
                table: "Products",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "Products",
                newName: "SizeType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizeType",
                table: "Products",
                newName: "UnitType");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Products",
                newName: "Units");
        }
    }
}
