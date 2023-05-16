using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSizeTypeToUnitType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SizeType",
                table: "Products",
                newName: "UnitType");

            migrationBuilder.RenameColumn(
                name: "SizeType",
                table: "CookedRecipeCalledIngredients",
                newName: "UnitType");

            migrationBuilder.RenameColumn(
                name: "SizeType",
                table: "CalledIngredients",
                newName: "UnitType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "Products",
                newName: "SizeType");

            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "CookedRecipeCalledIngredients",
                newName: "SizeType");

            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "CalledIngredients",
                newName: "SizeType");
        }
    }
}
