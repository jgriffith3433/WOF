using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CookedRecipesNullableProductStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CookedRecipeCalledIngredients_ProductStocks_ProductStockId",
                table: "CookedRecipeCalledIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "ProductStockId",
                table: "CookedRecipeCalledIngredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CookedRecipeCalledIngredients_ProductStocks_ProductStockId",
                table: "CookedRecipeCalledIngredients",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CookedRecipeCalledIngredients_ProductStocks_ProductStockId",
                table: "CookedRecipeCalledIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "ProductStockId",
                table: "CookedRecipeCalledIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CookedRecipeCalledIngredients_ProductStocks_ProductStockId",
                table: "CookedRecipeCalledIngredients",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
